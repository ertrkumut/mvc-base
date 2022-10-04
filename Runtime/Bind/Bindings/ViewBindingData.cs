using MVC.Runtime.Bind.Bindings.Mediator;
using MVC.Runtime.Contexts;

namespace MVC.Runtime.Bind.Bindings
{
    public struct ViewBindingData
    {
        public MediatorBinding Binding;
        public IContext Context;
    }
}