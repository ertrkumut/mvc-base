using System;

namespace MVC.Runtime.Signals
{
    public interface ISignalBody
    {
        #if UNITY_EDITOR
        internal string Name { get; set; }
        #endif

        internal Action<ISignalBody, object[]> InternalCallback { get; set; }
    }
}