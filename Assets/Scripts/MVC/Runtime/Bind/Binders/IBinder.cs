using MVC.Runtime.Bind.Bindings;

namespace MVC.Runtime.Bind.Binders
{
    public interface IBinder
    {
        IBinding Bind<TKeyType>();
        IBinding Bind(object key);
        IBinding GetBinding(object key);
        IBinding GetBinding<TKeyType>();
    }
}