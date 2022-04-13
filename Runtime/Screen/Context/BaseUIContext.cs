namespace MVC.Screen.Context
{
    public class BaseUIContext : Runtime.Contexts.Context
    {
        protected override void CoreBindings()
        {
            base.CoreBindings();

            InjectionBinderCrossContext.Bind<IScreenModel, ScreenModel>();
            MediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
        }
    }
}