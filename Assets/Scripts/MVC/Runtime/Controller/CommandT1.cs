namespace MVC.Runtime.Controller
{
    public abstract class Command<T1> : CommandBody, ICommand<T1>
    {
        public abstract void Execute(T1 param1);
    }
    
    public interface ICommand<T1> : ICommandBody
    {
        void Execute(T1 param1);
    }
}