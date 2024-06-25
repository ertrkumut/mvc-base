using System;
using MVC.Runtime.Function.Provider;

namespace MVC.Function.AsyncFunctions.DataContainer
{
    internal class AsyncFunctionDataContainer : AsyncFunctionDataContainerBase, IAsyncFunctionDataContainer
    {
        public Action FunctionCompletedCallback { get; set; }

        public IAsyncFunctionDataContainer AddFunctionCompletedCallback(Action callback)
        {
            FunctionCompletedCallback = callback;
            return this;
        }

        public void SetAsync()
        {
            CoroutineProvider.StartCoroutine(FunctionProvider.ExecuteAsyncFunction(this));
        }
    }
    
    public interface IAsyncFunctionDataContainer : IFunctionDataContainer
    {
        IAsyncFunctionDataContainer AddFunctionCompletedCallback(Action callback);
        void SetAsync();
    }
}