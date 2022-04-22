using MVC.Runtime.Screen.Pool;

namespace MVC.Runtime.Screen.Context
{
    public class BaseUIContext : Runtime.Contexts.Context
    {
        protected override void CoreBindings()
        {
            base.CoreBindings();

            InjectionBinderCrossContext.Bind<IScreenPoolController, ScreenPoolController>();
            InjectionBinderCrossContext.Bind<IScreenModel, ScreenModel>();
            
            MediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
        }
    }
}