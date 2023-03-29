using System.Collections.Generic;
using MVC.Runtime.Screen.Layer;
using MVC.Runtime.Screen.View;
using MVC.Runtime.ViewMediators.View;

namespace MVC.Runtime.Screen
{
    public interface IScreenManager : IView
    {
        int ManagerIndex { get; }
        ScreenLayer[] ScreenLayerList { get; }
        bool ShowScreen(IScreenBody screenBody);
        bool HideScreen(IScreenBody screenBody);
        List<IScreenBody> GetScreens(System.Enum screenType);
        List<IScreenBody> GetScreensInLayer(int layerIndex);
        List<IScreenBody> GetAllScreens();
        bool IsScreenContains(System.Enum screenType, int layerIndex);
    }
}