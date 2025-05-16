using System;

namespace MVC.Runtime.Signals
{
    public class Signal<T1, T2> : SignalBody, ISignal<T1, T2>
    {
        private event Action<T1, T2> _callbackOnce;
        private event Action<T1, T2> _callback;

        public void AddListenerOnce(Action<T1, T2> listener)
        {
            _callbackOnce += listener;
        }

        public void AddListener(Action<T1, T2> listener)
        {
            _callback += listener;
        }

        public void RemoveListener(Action<T1, T2> listener)
        {
            _callback -= listener;
            _callbackOnce -= listener;
        }

        public void Dispatch(T1 param1, T2 param2)
        {
            _callbackOnce?.Invoke(param1, param2);
            _callbackOnce = null;
            
            _internalCallback?.Invoke(this, new []
            {
                param1 as object,
                param2 as object
            });
            _callback?.Invoke(param1, param2);
        }
    }

    public interface ISignal<T1, T2> : ISignalBody
    {
        void AddListenerOnce(Action<T1, T2> listener);
        void AddListener(Action<T1, T2> listener);
        void RemoveListener(Action<T1, T2> listener);
        void Dispatch(T1 param1, T2 param2);
    }
}