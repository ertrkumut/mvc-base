using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    internal class TempScreenMediator : IMediator
    {
        [Inject] private TempScreenView _view { get; set; }
        
        public virtual void OnRegister()
        {
            _view.ScreenOpened += OnScreenOpened;
            _view.ScreenClosed += OnScreenClosed;
            //@Register
        }

        public virtual void OnRemove()
        {
            _view.ScreenOpened -= OnScreenOpened;
            _view.ScreenClosed -= OnScreenClosed;
            //@Remove
        }

        protected virtual void OnScreenOpened()
        {
            
        }

        protected virtual void OnScreenClosed()
        {
            
        }
        
        //@Methods
    }
}