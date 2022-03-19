using MVC.Runtime.Signals;

namespace MVC.Runtime.Controller.Binder
{
    public interface ICommandBinder
    {
        public CommandBinding Bind<TSignal>(TSignal key)
            where TSignal : ISignalBody;

        void ReleaseCommand(ICommandBody commandBody, params object[] commandParameters);
    }
}