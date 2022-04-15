using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;

namespace MVC.Runtime.Screen
{
    public class ScreenManagerMediator : IMediator
    {
        [Inject] protected IScreenModel _screenModel { get; set; }
        
        [Inject] protected ScreenManager _screenManager { get; set; }
        
        public virtual void OnRegister()
        {
            _screenModel.RegisterScreenManager(_screenManager);
        }

        public virtual void OnRemove()
        {
            _screenModel.UnRegisterScreenManager(_screenManager);
        }
    }
}