namespace MVC.Runtime.Controller
{
    public abstract class Command<T1, T2> : CommandBody, ICommand<T1, T2>
    {
        public abstract void Execute(T1 param1, T2 param2);
    }
    
    public interface ICommand<T1, T2> : ICommandBody
    {
        void Execute(T1 param1, T2 param2);
    }
}