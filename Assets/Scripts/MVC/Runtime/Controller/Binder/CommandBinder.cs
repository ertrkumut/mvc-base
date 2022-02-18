using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Signals;

namespace MVC.Runtime.Controller.Binder
{
    public class CommandBinder : Binder<CommandBinding>
    {
        public new virtual CommandBinding Bind<TSignal>(TSignal key)
            where TSignal : ISignalBody
        {
            return base.Bind(key);
        }
    }
}