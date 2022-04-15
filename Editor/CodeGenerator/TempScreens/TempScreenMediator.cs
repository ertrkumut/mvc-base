using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    internal class TempScreenMediator : IMediator
    {
        [Inject] private TempScreenView _view { get; set; }
        
        public virtual void OnRegister()
        {
            _view.OnScreenOpened += ScreenOpenedListener;
            _view.OnScreenClosed += ScreenClosedListener;
            //@Register
        }

        public virtual void OnRemove()
        {
            _view.OnScreenOpened -= ScreenOpenedListener;
            _view.OnScreenClosed -= ScreenClosedListener;
            //@Remove
        }

        protected virtual void ScreenOpenedListener()
        {
            
        }

        protected virtual void ScreenClosedListener()
        {
            
        }
    }
}