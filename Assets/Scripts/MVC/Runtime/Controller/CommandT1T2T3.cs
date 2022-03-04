namespace MVC.Runtime.Controller
{
    public abstract class Command<T1, T2, T3> : CommandBody, ICommand<T1, T2, T3>
    {
        public abstract void Execute(T1 param1, T2 param2, T3 param3);
    }
    
    public interface ICommand<T1, T2, T3> : ICommandBody
    {
        void Execute(T1 param1, T2 param2, T3 param3);
    }
}