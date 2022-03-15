namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class VoidFunction : FunctionBody, IVoidFunction
    {
        public abstract void Execute();
    }

    public interface IVoidFunction
    {
        void Execute();
    }
}