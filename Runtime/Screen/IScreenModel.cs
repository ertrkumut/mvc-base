using MVC.Runtime.Screen.Enum;
using MVC.Runtime.Screen.View;

namespace MVC.Runtime.Screen
{
    public interface IScreenModel
    {
        void RegisterScreenManager(ScreenManager screenManager);
        void UnRegisterScreenManager(ScreenManager screenManager);

        IScreenDataContainer NewScreen(System.Enum screenType);

        IScreenManager GetScreenManager(int managerId);

        void HideScreen(IScreenBody screenBody);
        void HideAllScreens(int screenManagerId = 0);
        
        ScreenState HasScreenOpen(System.Enum screenType, int screenManagerId = 0);

        TScreenType GetScreen<TScreenType>(System.Enum screenType, int screenManagerId = 0)
            where TScreenType : IScreenBody;
    }
}