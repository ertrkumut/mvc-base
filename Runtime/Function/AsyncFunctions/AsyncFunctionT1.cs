using System;
using System.Collections;

namespace MVC.Runtime.Function.AsyncFunctions
{
    public abstract class AsyncFunction<TParam1> : AsyncFunctionBody, IAsyncFunction<TParam1>
    {
        public Action<TParam1> FunctionCompletedCallback { get; set; }
    }

    public interface IAsyncFunction<TParam1>
    {
        Action<TParam1> FunctionCompletedCallback { get; set; }
        IEnumerator Execute();
    }
}