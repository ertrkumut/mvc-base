using System;

namespace MVC.Runtime.Signals
{
    public interface ISignalBody
    {
        internal string Name { get; set; }

        internal Action<ISignalBody, object[]> InternalCallback { get; set; }
    }
}