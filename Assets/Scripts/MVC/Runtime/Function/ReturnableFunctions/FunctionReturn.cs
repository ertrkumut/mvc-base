namespace MVC.Runtime.Function.ReturnableFunctions
{
    public abstract class FunctionReturn<TReturnType> : FunctionBody, IFunctionReturn<TReturnType>
    {
        public abstract TReturnType Execute();
    }

    public interface IFunctionReturn<TReturnType> : IFunctionBody
    {
        TReturnType Execute();
    }
}