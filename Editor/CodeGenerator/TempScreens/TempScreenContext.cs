using MVC.Runtime.Screen.Context;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    internal class TempScreenContext : BaseUIContext
    {
        public override void SignalBindings()
        {
            base.SignalBindings();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
            
            MediationBinder.Bind<TempScreenView>().To<TempScreenMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}