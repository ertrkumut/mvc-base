using MVC.Runtime.Screen.Enum;

namespace MVC.Runtime.Screen
{
    public interface IScreenDataContainer
    {
        IScreenDataContainer SetManagerIndex(int managerIndex);
        IScreenDataContainer SetLayer(ScreenLayerIndex layerIndex);
        IScreenDataContainer SetParameters(params object[] screenParameters);
    }
}