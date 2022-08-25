using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen
{
    public class ScreenDataContainer : IScreenDataContainer
    {
        private ScreenModel _screenModel;
    
        public System.Enum ScreenType;
        
        public int ManagerIndex;

        public ScreenLayerIndex LayerIndex;

        public object[] ScreenParameters;
        
        public ScreenDataContainer(ScreenModel screenModel)
        {
            _screenModel = screenModel;
            ManagerIndex = 0;
            LayerIndex = ScreenLayerIndex.Layer_1;
        }

        public IScreenDataContainer SetManagerIndex(int managerIndex = 0)
        {
            ManagerIndex = managerIndex;
            return this;
        }
        
        public IScreenDataContainer SetLayer(ScreenLayerIndex layerIndex = ScreenLayerIndex.Layer_0)
        {
            LayerIndex = layerIndex;
            return this;
        }

        public IScreenDataContainer SetParameters(params object[] screenParameters)
        {
            ScreenParameters = screenParameters;
            return this;
        }

        public TScreenType Show<TScreenType>()
            where TScreenType : MonoBehaviour, IScreenBody
        {
            var screen = _screenModel.CreateOrGetScreen<TScreenType>(this);
            MVCConsole.Log(ConsoleLogType.Screen, "Show Screen! type: " + screen.GetType().Name);
            return screen;
        }

        public IScreenBody Show()
        {
            var screen = _screenModel.CreateOrGetScreen<IScreenBody>(this);
            MVCConsole.Log(ConsoleLogType.Screen, "Show Screen! type: " + screen.GetType().Name);
            return screen;
        }
        
        public void Dispose()
        {
            ScreenType = null;
            ScreenParameters = null;
            
            ManagerIndex = 0;
            LayerIndex = ScreenLayerIndex.Layer_0;
        }
    }
}