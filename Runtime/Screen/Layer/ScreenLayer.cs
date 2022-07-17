using System.Collections.Generic;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen.Layer
{
    public class ScreenLayer : MonoBehaviour, IScreenLayer
    {
        [SerializeField] private ScreenLayerIndex _layerIndex;
        public ScreenLayerIndex LayerIndex => _layerIndex;
        
        public Dictionary<System.Enum, List<IScreenBody>> ScreensDict;

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
            
            Debug.LogError("Screen already in this layer! ScreenType: " + screenType + " Layer: " + _layerIndex);
            MVCConsole.LogError(ConsoleLogType.Screen, "Screen already in this layer! ScreenType: " + screenType + " Layer: " + _layerIndex);
            
            return false;
        }

        public bool RemoveScreen(IScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            if(!screens.Contains(screenBody))
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
        
        public List<IScreenBody> GetScreens()
        {
            var result = new List<IScreenBody>();

            foreach (var screenKeyValuePair in ScreensDict)
            {
                foreach (var screenBody in screenKeyValuePair.Value)
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
            if (ScreensDict == null)
                ScreensDict = new Dictionary<System.Enum, List<IScreenBody>>();
            
            if(!ScreensDict.ContainsKey(screenType))
                ScreensDict.Add(screenType, new List<IScreenBody>());
        }
    }
}