using System.Collections.Generic;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;

namespace MVC.Runtime.Screen.Layer
{
    public interface IScreenLayer
    {
        ScreenLayerIndex LayerIndex { get; }
        
        bool AddScreen(IScreenBody screenBody);
        bool RemoveScreen(IScreenBody screenBody);
        
        List<IScreenBody> GetScreens(System.Enum screenType);
        List<IScreenBody> GetAllScreens();
        
        bool IsScreenContains(System.Enum screenType);
        bool IsScreenContains(IScreenBody screenBody);
    }
}