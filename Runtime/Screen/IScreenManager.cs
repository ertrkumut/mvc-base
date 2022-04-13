using System.Collections.Generic;
using MVC.Screen.Enum;
using MVC.Screen.Layer;
using MVC.Screen.View;

namespace MVC.Screen
{
    public interface IScreenManager
    {
        int ManagerIndex { get; }
        
        ScreenLayer[] ScreenLayerList { get; }

        bool ShowScreen(IScreenBody screenBody);
        bool HideScreen(IScreenBody screenBody);

        List<IScreenBody> GetScreens(System.Enum screenType);
        bool IsScreenContains(System.Enum screenType, ScreenLayerIndex layerIndex);
    }
}