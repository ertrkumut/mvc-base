using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Runtime.Screen
{
    public interface IScreenDataContainer
    {
        IScreenDataContainer SetManagerIndex(int managerIndex = 0);
        IScreenDataContainer SetLayer(ScreenLayerIndex layerIndex = ScreenLayerIndex.Layer_1);
        IScreenDataContainer SetParameters(params object[] screenParameters);
        
        TScreenType Show<TScreenType>() where TScreenType : MonoBehaviour, IScreenBody;
        IScreenBody Show();
    }
}