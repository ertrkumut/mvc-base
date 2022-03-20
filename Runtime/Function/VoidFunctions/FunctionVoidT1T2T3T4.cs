namespace MVC.Runtime.Function.VoidFunctions
{
    public abstract class FunctionVoid<TParam1, TParam2, TParam3, TParam4> : FunctionBody, IFunctionVoid<TParam1, TParam2, TParam3, TParam4>
    {
        public abstract void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }

    public interface IFunctionVoid<TParam1, TParam2, TParam3, TParam4>
    {
        void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}