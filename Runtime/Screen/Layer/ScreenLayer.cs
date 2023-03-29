using System.Collections.Generic;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen.Layer
{
    public class ScreenLayer : MonoBehaviour, IScreenLayer
    {
        [SerializeField] private bool _isSafeAreaExists = true;
        public bool IsSafeAreaExists => _isSafeAreaExists;

        public Dictionary<System.Enum, List<IScreenBody>> ScreensDict = new();


        public bool AddScreen(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            if(!screens.Contains(screenBody))
            {
                ScreensDict[screenType].Add(screenBody);
                
                var rectTransform = screenBody.transform as RectTransform;
                rectTransform.SetParent(transform);
                rectTransform.localScale = Vector3.one;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                
                return true;
            }
            return false;
        }

        public bool RemoveScreen(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;

            if (!ScreensDict.ContainsKey(screenType))
                return false;
            
            var screens = ScreensDict[screenType];
                
            if(screens.Contains(screenBody))
            {
                ScreensDict[screenType].Remove(screenBody);
                return true;
            }

            return false;
        }

        public List<IScreenBody> GetScreens(System.Enum screenType)
        {
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            return screens;
        }
        
        public List<IScreenBody> GetAllScreens()
        {
            var result = new List<IScreenBody>();

            foreach (var screenKeyValue in ScreensDict.Values)
            {
                foreach (var screenBody in screenKeyValue)
                {
                    result.Add(screenBody);
                }
            }
            return result;
        }

        public bool IsScreenContains(System.Enum screenType)
        {
            var screens = GetScreens(screenType);
            return screens != null && screens.Count != 0;
        }

        public bool IsScreenContains(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            return screens.Contains(screenBody);
        }
        
        private void CreateDictionaryKeyIfIsNotExist(System.Enum screenType)
        {
            if(!ScreensDict.ContainsKey(screenType))
                ScreensDict.Add(screenType, new List<IScreenBody>());
        }
    }
}