using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Attributes;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen.Pool
{
    [HideInModelViewer]
    internal class ScreenPoolController : IScreenPoolController
    {
        private RectTransform _poolParent;

        protected Dictionary<System.Enum, List<IScreenBody>> _poolDict;
        protected Dictionary<System.Enum, List<IScreenBody>> _activeDict;
        
        public ScreenPoolController()
        {
            _poolParent = new GameObject("[MVC]_Screen Pool Parent").AddComponent<RectTransform>();
            Object.DontDestroyOnLoad(_poolParent.gameObject);
            
            _poolDict = new Dictionary<System.Enum, List<IScreenBody>>();
            _activeDict = new Dictionary<System.Enum, List<IScreenBody>>();
        }
        
        public IScreenBody GetScreenFromPool(System.Enum screenType)
        {
            var poolList = GetDisableScreenList(screenType);

            var availableScreenList = poolList
                .Where(x => x.ScreenState == ScreenState.InPool)
                .ToList();
            
            var availableScreen = availableScreenList.Count == 0 ? null : availableScreenList[0];
            if(availableScreen != null)
            {
                RemoveScreenFromPoolDict(availableScreen);
            }
            else
            {
                var prefab = Resources.Load<ScreenBody>("Screens/"+ screenType);
                availableScreen = Object.Instantiate(prefab, _poolParent);
            }

            availableScreen.ScreenType = screenType;
            AddScreenToActiveDict(availableScreen);
            
            return availableScreen;
        }

        public void SendScreenToPool(IScreenBody screenBody)
        {
            screenBody.transform.SetParent(_poolParent);
            RemoveScreenFromActiveDict(screenBody);
            AddScreenToPoolDict(screenBody);
        }

        public void RemoveScreenFromPoolDict(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            
            CreateNewDisableScreenList(screenType);

            _poolDict[screenType].Remove(screenBody);
        }

        public void AddScreenToPoolDict(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            
            CreateNewDisableScreenList(screenType);
            
            _poolDict[screenType].Add(screenBody);
        }

        public void RemoveScreenFromActiveDict(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            
            CreateNewActiveScreenList(screenType);

            _activeDict[screenType].Remove(screenBody);
        }
        
        public void AddScreenToActiveDict(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            
            CreateNewActiveScreenList(screenType);
            
            _activeDict[screenType].Add(screenBody);
        }
        
        protected List<IScreenBody> GetDisableScreenList(System.Enum screenType)
        {
            CreateNewDisableScreenList(screenType);

            return _poolDict[screenType];
        }

        protected List<IScreenBody> GetActiveScreenList(System.Enum screenType)
        {
            CreateNewActiveScreenList(screenType);

            return _activeDict[screenType];
        }

        protected void CreateNewActiveScreenList(System.Enum screenType)
        {
            if(!_activeDict.ContainsKey(screenType))
                _activeDict.Add(screenType, new List<IScreenBody>());
        }

        protected void CreateNewDisableScreenList(System.Enum screenType)
        {
            if(!_poolDict.ContainsKey(screenType))
                _poolDict.Add(screenType, new List<IScreenBody>());
        }
    }
}