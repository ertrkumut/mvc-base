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
        void HideScreen(int screenManagerId, System.Enum screenType);
        void HideScreenInLayer(int screenManagerId, int layerIndex);
        void HideAllScreens(int screenManagerId = 0);
        void HideAllScreensInAllManagers();
        
        ScreenState HasScreenOpen(System.Enum screenType, int screenManagerId = 0);

        TScreenType GetScreen<TScreenType>(System.Enum screenType, int screenManagerId = 0)
            where TScreenType : IScreenBody;

        void BackToHistory(int screenManagerId = 0);
        void ResetHistory(int screenManagerId);
        void ResetHistoryInAllScreenManagers();
    }
}