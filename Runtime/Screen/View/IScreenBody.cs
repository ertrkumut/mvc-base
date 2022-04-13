using MVC.Pool;
using MVC.Runtime.ViewMediators.View;

namespace MVC.Screen.View
{
    public interface IScreenBody : IView, IPoolable
    {
        /// <summary>
        /// It shows the state of the screen. None, InPool, InUse, InOpeningAnimation, InClosingAnimation
        /// </summary>
        ScreenState ScreenState { get; set; }

        /// <summary>
        /// It must be true, if there is custom opening animation like Timeline.
        /// If it's true, you need to manual Invoke ScreenOpened method
        /// </summary>
        bool CustomOpeningAnimation { get; }
        
        /// <summary>
        /// It must be true, if there is custom closing animation like Timeline.
        /// If it's true, you need to manual Invoke ScreenClosed method
        /// </summary>
        bool CustomClosingAnimation { get; }
    }
}