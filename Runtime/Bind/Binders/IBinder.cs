using MVC.Runtime.Bind.Bindings;

namespace MVC.Runtime.Bind.Binders
{
    public interface IBinder<TBindingType>
        where TBindingType : IBinding, new()
    {
        TBindingType Bind<TKeyType>();
        TBindingType Bind(object key);

        void UnBind(object key);
        void UnBindAll();

        TBindingType GetBinding(object key);
        TBindingType GetBinding<TKeyType>();
    }
}