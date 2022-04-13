using System.Collections.Generic;
using MVC.Screen.Enum;
using MVC.Screen.View;

namespace MVC.Screen.Layer
{
    public interface IScreenLayer
    {
        ScreenLayerIndex LayerIndex { get; }
        
        bool AddScreen(IScreenBody screenBody);
        bool RemoveScreen(IScreenBody screenBody);
        
        List<IScreenBody> GetScreens(System.Enum screenType);
        List<IScreenBody> GetScreens();
        
        bool IsScreenContains(System.Enum screenType);
        bool IsScreenContains(IScreenBody screenBody);
    }
}