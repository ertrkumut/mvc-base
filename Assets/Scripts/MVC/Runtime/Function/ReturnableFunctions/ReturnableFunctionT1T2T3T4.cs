namespace MVC.Runtime.Function
{
    public abstract class ReturnableFunction<TReturnType, TParam1, TParam2, TParam3, TParam4> : FunctionBody, IReturnableFunction<TReturnType, TParam1, TParam2, TParam3, TParam4>
    {
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
    
    public interface IReturnableFunction<TReturnType, TParam1, TParam2, TParam3, TParam4> : IFunctionBody
    {
        TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}