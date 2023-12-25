using System;

namespace MVC.Runtime.Signals
{
    public class Signal<T1> : SignalBody, ISignal<T1>
    {
        private event Action<T1> _callbackOnce;
        private event Action<T1> _callback;

        public void AddListenerOnce(Action<T1> listener)
        {
            _callbackOnce += listener;
        }

        public void AddListener(Action<T1> listener)
        {
            _callback += listener;
        }

        public void RemoveListener(Action<T1> listener)
        {
            _callback -= listener;
        }

        public void Dispatch(T1 param)
        {
            _callbackOnce?.Invoke(param);
            _callbackOnce = null;
            
            _internalCallback?.Invoke(this, new []
            {
                param as object
            });
            _callback?.Invoke(param);
        }
    }

    public interface ISignal<T> : ISignalBody
    {
        void AddListenerOnce(Action<T> listener);
        void AddListener(Action<T> listener);
        void RemoveListener(Action<T> listener);
        void Dispatch(T param);
    }
}