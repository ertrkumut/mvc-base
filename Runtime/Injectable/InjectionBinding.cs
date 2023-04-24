using MVC.Runtime.Bind.Bindings;
using MVC.Runtime.Contexts;

namespace MVC.Runtime.Injectable
{
    public class InjectionBinding : Binding
    {
        public string Name;
        public IContext BindedContext;
    }
}