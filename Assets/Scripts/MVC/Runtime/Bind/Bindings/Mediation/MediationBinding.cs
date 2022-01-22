using MVC.Runtime.ViewMediators.Mediator;

namespace MVC.Runtime.Bind.Bindings.Mediation
{
    public class MediationBinding : Binding
    {
        public new virtual void To<TValueType>()
            where TValueType : IMVCMediator
        {
            base.To<TValueType>();
        }
    }
}