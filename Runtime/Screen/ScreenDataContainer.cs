using MVC.Runtime.Attributes;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen
{
    public class ScreenDataContainer : IScreenDataContainer
    {
        private ScreenModel _screenModel;
    
        public int ManagerIndex;
        public System.Enum ScreenType;
        public int LayerIndex;
        public bool HidedFromHistory;

        [HideInModelViewer] public object[] ScreenParameters;
        
        public ScreenDataContainer(ScreenModel screenModel)
        {
            _screenModel = screenModel;
            ManagerIndex = 0;
            LayerIndex = 0;
            HidedFromHistory = true;
        }

        public IScreenDataContainer SetManagerIndex(int managerIndex = 0)
        {
            ManagerIndex = managerIndex;
            return this;
        }
        
        public IScreenDataContainer SetLayer(int layerIndex = 0)
        {
            LayerIndex = layerIndex;
            return this;
        }

        public IScreenDataContainer SetParameters(params object[] screenParameters)
        {
            ScreenParameters = screenParameters;
            return this;
        }

        public IScreenDataContainer AddToHistory()
        {
            HidedFromHistory = false;
            return this;
        }

        public TScreenType Show<TScreenType>()
            where TScreenType : MonoBehaviour, IScreenBody
        {
            var screen = _screenModel.ShowScreen(this);
            return screen as TScreenType;
        }

        public IScreenBody Show()
        {
            var screen = _screenModel.ShowScreen(this);
            return screen;
        }
        
        public void Dispose()
        {
            ScreenType = null;
            ScreenParameters = null;
            
            ManagerIndex = 0;
            LayerIndex = 0;
            HidedFromHistory = true;
        }
    }
}