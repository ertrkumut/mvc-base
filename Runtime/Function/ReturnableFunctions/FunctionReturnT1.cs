namespace MVC.Runtime.Function.ReturnableFunctions
{
    public abstract class FunctionReturn<TReturnType, TParam1> : FunctionBody, IFunctionReturn<TReturnType, TParam1>
    {
        public abstract TReturnType Execute(TParam1 param1);
    }
    
    public interface IFunctionReturn<TReturnType, TParam1> : IFunctionBody
    {
        TReturnType Execute(TParam1 param1);
    }
}