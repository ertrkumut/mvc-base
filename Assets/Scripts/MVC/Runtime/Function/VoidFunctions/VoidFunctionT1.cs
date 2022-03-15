namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class VoidFunction<TParam1> : FunctionBody, IVoidFunction<TParam1>
    {
        public abstract void Execute(TParam1 param1);
    }

    public interface IVoidFunction<TParam1>
    {
        void Execute(TParam1 param1);
    }
}