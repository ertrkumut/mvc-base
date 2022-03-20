namespace MVC.Runtime.Controller
{
    public abstract class Command<T1, T2, T3, T4> : CommandBody, ICommand<T1, T2, T3, T4>
    {
        public abstract void Execute(T1 param1, T2 param2, T3 param3, T4 param4);
    }
    
    public interface ICommand<T1, T2, T3, T4> : ICommandBody
    {
        void Execute(T1 param1, T2 param2, T3 param3, T4 param4);
    }
}