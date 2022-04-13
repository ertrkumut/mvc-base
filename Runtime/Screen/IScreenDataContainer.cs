using MVC.Screen.Enum;

namespace MVC.Screen
{
    public interface IScreenDataContainer
    {
        IScreenDataContainer SetManagerIndex(int managerIndex);
        IScreenDataContainer SetLayer(ScreenLayerIndex layerIndex);
        IScreenDataContainer SetParameters(params object[] screenParameters);
    }
}