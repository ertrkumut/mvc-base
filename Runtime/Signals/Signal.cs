using System;

namespace MVC.Runtime.Signals
{
    public class Signal : SignalBody, ISignal
    {
        private event Action _callbackOnce;
        private event Action _callback;

        public void AddListenerOnce(Action listener)
        {
            _callbackOnce += listener;
        }
        
        public void AddListener(Action listener)
        {
            _callback += listener;
        }

        public void RemoveListener(Action listener)
        {
            _callback -= listener;
        }

        public void Dispatch()
        {
            _callbackOnce?.Invoke();
            _callbackOnce = null;
            
            _internalCallback?.Invoke(this, null);
            _callback?.Invoke();
        }
    }

    public interface ISignal : ISignalBody
    {
        void AddListenerOnce(Action listener);
        void AddListener(Action listener);
        void RemoveListener(Action listener);
        void Dispatch();
    }
}