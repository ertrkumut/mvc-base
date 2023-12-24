using System;
using MVC.Runtime.Function.Provider;

namespace MVC.Function.AsyncFunctions.DataContainer
{
    internal class AsyncFunctionDataContainer<TParam1> : AsyncFunctionDataContainerBase, IAsyncFunctionDataContainer<TParam1>
    {
        public Action<TParam1> FunctionCompletedCallback { get; set; }

        public IAsyncFunctionDataContainer<TParam1> AddFunctionCompletedCallback(Action<TParam1> callback)
        {
            FunctionCompletedCallback = callback;
            return this;
        }

        public void SetAsync()
        {
            CoroutineProvider.StartCoroutine(FunctionProvider.ExecuteAsyncFunction(this));
        }
    }
    
    public interface IAsyncFunctionDataContainer<TParam1> : IFunctionDataContainer
    {
        IAsyncFunctionDataContainer<TParam1> AddFunctionCompletedCallback(Action<TParam1> callback);
        void SetAsync();
    }
}