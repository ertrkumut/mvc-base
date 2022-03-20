namespace MVC.Runtime.Controller
{
    public abstract class Command : CommandBody, ICommand
    {
        public abstract void Execute();
    }

    public interface ICommand : ICommandBody
    {
        void Execute();
    }
}