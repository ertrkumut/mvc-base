namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class FunctionVoid<TParam1, TParam2> : FunctionBody, IFunctionVoid<TParam1, TParam2>
    {
        public abstract void Execute(TParam1 param1, TParam2 param2);
    }

    public interface IFunctionVoid<TParam1, TParam2>
    {
        void Execute(TParam1 param1, TParam2 param2);
    }
}