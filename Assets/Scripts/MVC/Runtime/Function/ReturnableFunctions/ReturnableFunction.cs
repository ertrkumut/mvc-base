namespace MVC.Runtime.Function
{
    public abstract class ReturnableFunction<TReturnType> : FunctionBody, IReturnableFunction<TReturnType>
    {
        public abstract TReturnType Execute();
    }

    public interface IReturnableFunction<TReturnType> : IFunctionBody
    {
        TReturnType Execute();
    }
}