using System.Collections.Generic;
using System.Linq;
using MVC.Screen.Enum;
using MVC.Screen.View;
using UnityEngine;

namespace MVC.Screen.Layer
{
    public class ScreenLayer : MonoBehaviour, IScreenLayer
    {
        private ScreenLayerIndex _layerIndex;
        public ScreenLayerIndex LayerIndex => _layerIndex;
        
        public Dictionary<System.Enum, List<ScreenBody>> ScreensDict;

        public void AddScreen(ScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            if(!screens.Contains(screenBody))
                ScreensDict[screenType].Add(screenBody);
            else
                Debug.LogError("Screen already in this layer! ScreenType: " + screenType + " Layer: " + _layerIndex);
        }

        public void RemoveScreen(ScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            if(!screens.Contains(screenBody))
                ScreensDict[screenType].Remove(screenBody);
        }

        public List<ScreenBody> GetScreens(System.Enum screenType)
        {
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            return screens;
        }

        public bool IsScreenContains(System.Enum screenType)
        {
            var screens = GetScreens(screenType);
            return screens != null && screens.Count != 0;
        }

        public bool IsScreenContains(ScreenBody screenBody)
        {
            var screenType = screenBody.ScreenType;
            CreateDictionaryKeyIfIsNotExist(screenType);

            var screens = ScreensDict[screenType];
            return screens.Contains(screenBody);
        }
        
        private void CreateDictionaryKeyIfIsNotExist(System.Enum screenType)
        {
            if(!ScreensDict.ContainsKey(screenType))
                ScreensDict.Add(screenType, new List<ScreenBody>());
        }
    }
}