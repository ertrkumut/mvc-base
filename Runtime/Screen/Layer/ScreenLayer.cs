using System.Collections.Generic;
using MVC.Screen.Enum;
using MVC.Screen.View;
using UnityEngine;

namespace MVC.Screen.Layer
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
                return true;
            }
            
            Debug.LogError("Screen already in this layer! ScreenType: " + screenType + " Layer: " + _layerIndex);
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
            if(!ScreensDict.ContainsKey(screenType))
                ScreensDict.Add(screenType, new List<IScreenBody>());
        }
    }
}