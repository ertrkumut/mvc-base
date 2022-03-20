namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class FunctionVoid : FunctionBody, IFunctionVoid
    {
        public abstract void Execute();
    }

    public interface IFunctionVoid
    {
        void Execute();
    }
}