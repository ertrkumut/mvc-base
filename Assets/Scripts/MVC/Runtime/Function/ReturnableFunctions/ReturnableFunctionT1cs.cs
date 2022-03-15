namespace MVC.Runtime.Function
{
    public abstract class ReturnableFunction<TReturnType, TParam1> : FunctionBody, IReturnableFunction<TReturnType, TParam1>
    {
        public abstract TReturnType Execute(TParam1 param1);
    }
    
    public interface IReturnableFunction<TReturnType, TParam1> : IFunctionBody
    {
        TReturnType Execute(TParam1 param1);
    }
}