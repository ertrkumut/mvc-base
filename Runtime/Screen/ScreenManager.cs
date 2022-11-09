using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.Layer;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen
{
    [RequireComponent(typeof(ViewInjector))]
    public class ScreenManager : MonoBehaviour, IScreenManager
    {
        public bool IsRegistered { get; set; }
        
        [SerializeField] private int _managerIndex;
        public int ManagerIndex => _managerIndex;

        [SerializeField] private ScreenLayer[] _screenLayerList;
        public ScreenLayer[] ScreenLayerList => _screenLayerList;

        public bool ShowScreen(IScreenBody screenBody)
        {
            var layerIndex = screenBody.LayerIndex;
            var layer = GetLayer(layerIndex);

            if (layer == null)
            {
                Debug.LogError("Screen can not open, cause there is no layer! ScreenType: " + screenBody.ScreenType);
                return false;
            }

            return layer.AddScreen(screenBody);
        }

        public bool HideScreen(IScreenBody screenBody)
        {
            var layerIndex = screenBody.LayerIndex;
            var layer = GetLayer(layerIndex);
            
            if (layer == null)
            {
                Debug.LogError("Screen can not hide, cause there is no layer! ScreenType: " + screenBody.ScreenType);
                return false;
            }
            
            return layer.RemoveScreen(screenBody);
        }

        public List<IScreenBody> GetScreens(System.Enum screenType)
        {
            var result = _screenLayerList
                .SelectMany(layer => layer.GetScreens(screenType))
                .ToList();

            return result;
        }
        
        public List<IScreenBody> GetScreensInLayer(ScreenLayerIndex layerIndex)
        {
            var layer = GetLayer(layerIndex);
            return layer.GetAllScreens();
        }

        public List<IScreenBody> GetAllScreens()
        {
            var result = _screenLayerList
                .SelectMany(layer => layer.GetAllScreens())
                .ToList();
            
            return result;
        }

        public bool IsScreenContains(System.Enum screenType, ScreenLayerIndex layerIndex)
        {
            var layer = GetLayer(layerIndex);
            
            return layer != null && layer.IsScreenContains(screenType);
        }
        
        protected ScreenLayer GetLayer(ScreenLayerIndex layerIndex)
        {
            var layer = _screenLayerList.FirstOrDefault(x => x.LayerIndex == layerIndex);
            if(layer == null)
                Debug.LogError("Layer not found! ManagerIndex: " + _managerIndex + " LayerIndex: " + layerIndex);
            
            return layer;
        }
    }
}