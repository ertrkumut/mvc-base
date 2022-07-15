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
        
        #if UNITY_EDITOR
        
        protected string _name;

        string ISignalBody.Name
        {
            get => _name;
            set => _name = value;
        }
        
        #endif
    }
}