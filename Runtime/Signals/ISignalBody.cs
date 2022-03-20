using System;

namespace MVC.Runtime.Signals
{
    public interface ISignalBody
    {
        internal Action<ISignalBody, object[]> InternalCallback { get; set; }
    }
}