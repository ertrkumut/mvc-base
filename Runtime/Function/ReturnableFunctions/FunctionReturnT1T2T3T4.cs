namespace MVC.Runtime.Function.ReturnableFunctions
{
    public abstract class FunctionReturn<TReturnType, TParam1, TParam2, TParam3, TParam4> : FunctionBody, IFunctionReturn<TReturnType, TParam1, TParam2, TParam3, TParam4>
    {
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
    
    public interface IFunctionReturn<TReturnType, TParam1, TParam2, TParam3, TParam4> : IFunctionBody
    {
        TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}