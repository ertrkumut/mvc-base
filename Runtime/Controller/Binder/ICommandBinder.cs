using MVC.Runtime.Signals;

namespace MVC.Runtime.Controller.Binder
{
    public interface ICommandBinder
    {
        public CommandBinding Bind<TSignal>(TSignal key)
            where TSignal : ISignalBody;

        void ReleaseCommand(ICommandBody commandBody, params object[] commandParameters);

        void Jump<TCommandType>(ICommandBody commandBody, params object[] commandParameters)
            where TCommandType : ICommandBody;
        
        void StopCommand(ICommandBody commandBody);
    }
}