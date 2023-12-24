using System.Collections;

namespace MVC.Runtime.Function.AsyncFunctions
{
    public abstract class AsyncFunctionBody : FunctionBody
    {
        public abstract IEnumerator Execute();
    }
}