namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class VoidFunction<TParam1, TParam2> : FunctionBody, IVoidFunction<TParam1, TParam2>
    {
        public abstract void Execute(TParam1 param1, TParam2 param2);
    }

    public interface IVoidFunction<TParam1, TParam2>
    {
        void Execute(TParam1 param1, TParam2 param2);
    }
}