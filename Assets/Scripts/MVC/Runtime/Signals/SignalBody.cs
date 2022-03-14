using System;

namespace MVC.Runtime.Signals
{
    public class SignalBody : ISignalBody
    {
        protected Action<ISignalBody, object[]> _internalCallback;

        Action<ISignalBody, object[]> ISignalBody.InternalCallback
        {
            get => _internalCallback;
            set => _internalCallback = value;
        }
    }
}