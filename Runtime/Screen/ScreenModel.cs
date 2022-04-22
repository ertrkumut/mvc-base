using System;
using System.Collections.Generic;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Screen.Pool;
using MVC.Runtime.Screen.View;
using MVC.Runtime.ViewMediators.Utils;
using UnityEngine;

namespace MVC.Runtime.Screen
{
    public class ScreenModel : IScreenModel
    {
        private Dictionary<int, IScreenManager> _screenManagerDict;

        private List<ScreenDataContainer> _screenDataContainerPool;

        [Inject] protected IScreenPoolController _screenPoolController;
        
        [PostConstruct]
        protected void PostConstruct()
        {
            _screenManagerDict = new Dictionary<int, IScreenManager>();
            _screenDataContainerPool = new List<ScreenDataContainer>();
        }

        public void RegisterScreenManager(ScreenManager screenManager)
        {
            if(!_screenManagerDict.ContainsKey(screenManager.ManagerIndex))
                _screenManagerDict.Add(screenManager.ManagerIndex, screenManager);
        }

        public void UnRegisterScreenManager(ScreenManager screenManager)
        {
            if(_screenManagerDict.ContainsKey(screenManager.ManagerIndex))
                _screenManagerDict.Remove(screenManager.ManagerIndex);
        }

        public IScreenDataContainer NewScreen(System.Enum screenType)
        {
            var dataContainer = GetAvailableScreenDataContainer();
            dataContainer.ScreenType = screenType;
            return dataContainer;
        }

        public void HideScreen(IScreenBody screenBody)
        {
            
        }
        
        internal TScreenType CreateOrGetScreen<TScreenType>(ScreenDataContainer screenDataContainer)
            where TScreenType : MonoBehaviour, IScreenBody
        {
            var screenManager = GetScreenManager(screenDataContainer.ManagerIndex);

            var availableScreen = _screenPoolController.GetScreenFromPool(screenDataContainer.ScreenType);
            screenManager.ShowScreen(availableScreen);

            availableScreen.InjectView();
            ((ScreenBody) availableScreen).Open();
            
            _screenDataContainerPool.Add(screenDataContainer);
            screenDataContainer.Dispose();
            
            return (TScreenType) availableScreen;
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
    }

    public interface IScreenModel
    {
        void RegisterScreenManager(ScreenManager screenManager);
        void UnRegisterScreenManager(ScreenManager screenManager);

        IScreenDataContainer NewScreen(System.Enum screenType);

        IScreenManager GetScreenManager(int managerId);
    }
}