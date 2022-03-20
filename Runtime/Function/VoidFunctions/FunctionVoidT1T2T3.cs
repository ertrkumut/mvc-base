namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class FunctionVoid<TParam1, TParam2, TParam3> : FunctionBody, IFunctionVoid<TParam1, TParam2, TParam3>
    {
        public abstract void Execute(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface IFunctionVoid<TParam1, TParam2, TParam3>
    {
        void Execute(TParam1 param1, TParam2 param2, TParam3 param3);
    }
}