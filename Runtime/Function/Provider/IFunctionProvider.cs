using MVC.Function.AsyncFunctions.DataContainer;
using MVC.Runtime.Function.AsyncFunctions;

namespace MVC.Runtime.Function.Provider
{
    public interface IFunctionProvider
    {
        IFunctionDataContainer Execute<TFunctionType>() where TFunctionType : IFunctionBody;
        
        IAsyncFunctionDataContainer ExecuteAsync<TFunctionType>() where TFunctionType : IAsyncFunction;
        IAsyncFunctionDataContainer<TParam1> ExecuteAsync<TFunctionType, TParam1>() where TFunctionType : IAsyncFunction<TParam1>;
    }
}