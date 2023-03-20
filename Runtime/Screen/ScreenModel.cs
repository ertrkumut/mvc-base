using System;
using System.Collections.Generic;
using MVC.Editor.Console;
using MVC.Runtime.Attributes;
using MVC.Runtime.Console;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Pool;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.Pool;
using MVC.Runtime.Screen.View;
using MVC.Runtime.ViewMediators.Utils;

namespace MVC.Runtime.Screen
{
    public class ScreenModel : IScreenModel
    {
        private Dictionary<int, IScreenManager> _screenManagerDict;

        [ShowInModelViewer] private Dictionary<int, List<ScreenHistoryData>> _historyDataList;
        private List<ScreenDataContainer> _screenDataContainerPool;

        [Inject] protected IScreenPoolController _screenPoolController;

        [PostConstruct]
        protected void PostConstruct()
        {
            _screenManagerDict = new Dictionary<int, IScreenManager>();
            _historyDataList = new Dictionary<int, List<ScreenHistoryData>>();
            _screenDataContainerPool = new List<ScreenDataContainer>();
        }

        #region Register-ScreenManager

        public void RegisterScreenManager(ScreenManager screenManager)
        {
            if(!_screenManagerDict.ContainsKey(screenManager.ManagerIndex))
            {
                _screenManagerDict.Add(screenManager.ManagerIndex, screenManager);
                MVCConsole.Log(ConsoleLogType.Screen, "New Screen Registered! id: " + screenManager.ManagerIndex);
            }
        }

        public void UnRegisterScreenManager(ScreenManager screenManager)
        {
            if(_screenManagerDict.ContainsKey(screenManager.ManagerIndex))
            {
                _screenManagerDict.Remove(screenManager.ManagerIndex);
                MVCConsole.Log(ConsoleLogType.Screen, "Screen UnRegistered! id: " + screenManager.ManagerIndex);
            }
        }

        #endregion

        #region NewScreen-HideScreen

        public IScreenDataContainer NewScreen(System.Enum screenType)
        {
            var dataContainer = GetAvailableScreenDataContainer();
            dataContainer.ScreenType = screenType;
            return dataContainer;
        }

        public void HideScreen(IScreenBody screenBody)
        {
            var screenManagerId = screenBody.ScreenManagerId;
            var screenManager = GetScreenManager(screenManagerId);

            screenManager.HideScreen(screenBody);
            (screenBody as ScreenBody)?.Close();
        }

        public void HideScreen(int screenManagerId, System.Enum screenType)
        {
            var screenManager = GetScreenManager(screenManagerId);
            var screens = screenManager.GetScreens(screenType);
            
            foreach (var screenBody in screens)
            {
                HideScreen(screenBody);
            }
        }

        public void HideScreenInLayer(int screenManagerId, ScreenLayerIndex layerIndex)
        {
            var screenManager = GetScreenManager(screenManagerId);
            var screens = screenManager.GetScreensInLayer(layerIndex);

            foreach (var screenBody in screens)
            {
                HideScreen(screenBody);
            }
        }
        
        public void HideAllScreens(int screenManagerId = 0)
        {
            var screenManager = GetScreenManager(screenManagerId);

            var allScreens = screenManager.GetAllScreens();
            foreach (var screenBody in allScreens)
            {
                HideScreen(screenBody);
            }
        }

        public void HideAllScreensInAllManagers()
        {
            foreach (var screenManager in _screenManagerDict)
            {
                HideAllScreens(screenManager.Key);
            }
        }

        public ScreenState HasScreenOpen(System.Enum screenType, int screenManagerId = 0)
        {
            var screenManager = GetScreenManager(screenManagerId);
            var screen = screenManager.GetScreens(screenType);
            return screen.Count == 0 ? ScreenState.None : screen[0].ScreenState;
        }

        public TScreenType GetScreen<TScreenType>(System.Enum screenType, int screenManagerId = 0)
            where TScreenType : IScreenBody
        {
            var screenManager = GetScreenManager(screenManagerId);
            var screen = screenManager.GetScreens(screenType);
            return screen.Count == 0 ? default : (TScreenType) screen[0];
        }

        internal IScreenBody ShowScreen(ScreenDataContainer screenDataContainer)
        {
            var screen = CreateOrGetScreen<IScreenBody>(screenDataContainer);
            MVCConsole.Log(ConsoleLogType.Screen, $"Show Screen! \ntype: {screenDataContainer.ScreenType} \nManager; {screenDataContainer.ManagerIndex} \nLayer: {screenDataContainer.LayerIndex}");

            if (!screenDataContainer.HidedFromHistory && screen is IScreenView)
                AddScreenToHistory(screenDataContainer);
            
            screenDataContainer.Dispose();
            return screen;
        }
        
        #endregion

        internal TScreenType CreateOrGetScreen<TScreenType>(ScreenDataContainer screenDataContainer)
            where TScreenType : IScreenBody
        {
            var screenManager = GetScreenManager(screenDataContainer.ManagerIndex);

            var availableScreen = _screenPoolController.GetScreenFromPool(screenDataContainer.ScreenType);
            availableScreen.LayerIndex = screenDataContainer.LayerIndex;
            screenManager.ShowScreen(availableScreen);

            availableScreen.Register();
            ((ScreenBody) availableScreen).InitializeScreenParams(screenDataContainer.ScreenParameters);
            ((ScreenBody) availableScreen).ReturnToPoolAction = OnReturnToPoolAction;
            ((ScreenBody) availableScreen).Open();
            
            _screenDataContainerPool.Add(screenDataContainer);

            return (TScreenType) availableScreen;
        }

        private void OnReturnToPoolAction(IPoolable poolItem)
        {
            _screenPoolController.SendScreenToPool((ScreenBody) poolItem);
        }
        
        public IScreenManager GetScreenManager(int managerId)
        {
            if (!_screenManagerDict.ContainsKey(managerId))
            {
                throw new Exception("There is no screen manager!! - Id: " + managerId);
            }

            return _screenManagerDict[managerId];
        }
        
        private ScreenDataContainer GetAvailableScreenDataContainer()
        {
            ScreenDataContainer dataContainer;

            if (_screenDataContainerPool.Count > 0)
            {
                dataContainer = _screenDataContainerPool[0];
                _screenDataContainerPool.Remove(dataContainer);
            }
            else
                dataContainer = new ScreenDataContainer(this);
            
            return dataContainer;
        }
        
        protected void AddScreenToHistory(ScreenDataContainer screenDataContainer)
        {
            var history = new ScreenHistoryData(
                screenDataContainer.ManagerIndex,
                screenDataContainer.ScreenType,
                screenDataContainer.LayerIndex);
            var screenManagerId = screenDataContainer.ManagerIndex;
            
            if(!_historyDataList.ContainsKey(screenManagerId))
                _historyDataList.Add(screenManagerId, new List<ScreenHistoryData>());
            
            _historyDataList[screenManagerId].Add(history);
        }

        public virtual void BackToHistory(int screenManagerId = 0)
        {
            var historyDataList = GetScreenHistoryData(screenManagerId);
            if(historyDataList == null)
            {
                MVCConsole.LogError(ConsoleLogType.Screen, $"There is no screen history! ScreenManagerId: {screenManagerId}");
                return;
            }

            var lastItem = historyDataList[^1];
            historyDataList.Remove(lastItem);

            NewScreen(lastItem.ScreenType)
                .SetManagerIndex(lastItem.ManagerIndex)
                .SetLayer(lastItem.LayerIndex)
                .Show();
        }

        internal List<ScreenHistoryData> GetScreenHistoryData(int screenManagerId)
        {
            return _historyDataList.ContainsKey(screenManagerId) ? _historyDataList[screenManagerId] : null;
        }

        public void ResetHistory(int screenManagerId = 0)
        {
            if (_historyDataList.ContainsKey(screenManagerId))
                _historyDataList.Remove(screenManagerId);
        }

        public void ResetHistoryInAllScreenManagers()
        {
            foreach (var historyDataList in _historyDataList)
            {
                ResetHistory(historyDataList.Key);
            }
        }
    }
}