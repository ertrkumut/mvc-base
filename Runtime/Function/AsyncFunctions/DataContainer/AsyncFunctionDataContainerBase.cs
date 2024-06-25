using MVC.Runtime.Function.Provider;
using MVC.Runtime.Provider.Coroutine;

namespace MVC.Function.AsyncFunctions.DataContainer
{
    internal class AsyncFunctionDataContainerBase : FunctionDataContainer
    {
        public ICoroutineProvider CoroutineProvider;
    }
}