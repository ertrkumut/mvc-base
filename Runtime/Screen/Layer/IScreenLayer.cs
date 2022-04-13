using System.Collections.Generic;
using MVC.Screen.Enum;
using MVC.Screen.View;

namespace MVC.Screen.Layer
{
    public interface IScreenLayer
    {
        ScreenLayerIndex LayerIndex { get; }
        
        void AddScreen(ScreenBody screenBody);
        void RemoveScreen(ScreenBody screenBody);
        
        List<ScreenBody> GetScreens(System.Enum screenType);
        
        bool IsScreenContains(System.Enum screenType);
        bool IsScreenContains(ScreenBody screenBody);
    }
}