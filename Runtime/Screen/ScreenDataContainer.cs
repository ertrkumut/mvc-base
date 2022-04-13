using MVC.Screen.Enum;

namespace MVC.Screen
{
    public class ScreenDataContainer : IScreenDataContainer
    {
        public System.Enum ScreenType;
        
        public int ManagerIndex;

        public ScreenLayerIndex LayerIndex;

        public object[] ScreenParameters;
        
        public ScreenDataContainer(System.Enum screenType)
        {
            ScreenType = screenType;
        }

        public IScreenDataContainer SetManagerIndex(int managerIndex)
        {
            ManagerIndex = managerIndex;
            return this;
        }
        
        public IScreenDataContainer SetLayer(ScreenLayerIndex layerIndex)
        {
            LayerIndex = layerIndex;
            return this;
        }

        public IScreenDataContainer SetParameters(params object[] screenParameters)
        {
            ScreenParameters = screenParameters;
            return this;
        }
    }

    public interface IScreenDataContainer
    {
        IScreenDataContainer SetManagerIndex(int managerIndex);
        IScreenDataContainer SetLayer(ScreenLayerIndex layerIndex);
        IScreenDataContainer SetParameters(params object[] screenParameters);
    }
}