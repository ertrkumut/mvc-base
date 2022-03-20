namespace MVC.Runtime.Function.ReturnableFunctions
{
    public abstract class FunctionReturn<TReturnType, TParam1, TParam2> : FunctionBody, IFunctionReturn<TReturnType, TParam1, TParam2>
    {
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2);
    }
    
    public interface IFunctionReturn<TReturnType, TParam1, TParam2> : IFunctionBody
    {
        TReturnType Execute(TParam1 param1, TParam2 param2);
    }
}