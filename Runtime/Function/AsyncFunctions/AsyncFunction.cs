using System;
using System.Collections;

namespace MVC.Runtime.Function.AsyncFunctions
{
    public abstract class AsyncFunction : AsyncFunctionBody, IAsyncFunction
    {
        public Action FunctionCompletedCallback { get; set; }
    }

    public interface IAsyncFunction
    {
        Action FunctionCompletedCallback { get; set; }
        IEnumerator Execute();
    }
}