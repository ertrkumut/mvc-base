using MVC.Runtime.Screen.Pool;

namespace MVC.Runtime.Screen.Context
{
    public class BaseUIContext : Runtime.Contexts.Context
    {
        protected IScreenModel _screenModel;
        
        protected override void CoreBindings()
        {
            base.CoreBindings();

            _screenModel = InjectionBinderCrossContext.Bind<IScreenModel, ScreenModel>();
            
            InjectionBinderCrossContext.Bind<IScreenPoolController, ScreenPoolController>();

            MediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
        }
    }
}