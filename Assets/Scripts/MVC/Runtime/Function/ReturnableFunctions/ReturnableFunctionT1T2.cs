namespace MVC.Runtime.Function
{
    public abstract class ReturnableFunction<TReturnType, TParam1, TParam2> : FunctionBody, IReturnableFunction<TReturnType, TParam1, TParam2>
    {
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2);
    }
    
    public interface IReturnableFunction<TReturnType, TParam1, TParam2> : IFunctionBody
    {
        TReturnType Execute(TParam1 param1, TParam2 param2);
    }
}