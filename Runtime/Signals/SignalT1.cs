using System;

namespace MVC.Runtime.Signals
{
    public class Signal<T1> : SignalBody, ISignal<T1>
    {
        private event Action<T1> callback; 
        
        public void AddListener(Action<T1> listener)
        {
            callback += listener;
        }

        public void RemoveListener(Action<T1> listener)
        {
            callback -= listener;
        }

        public void Dispatch(T1 param)
        {
            _internalCallback?.Invoke(this, new []
            {
                param as object
            });
            callback?.Invoke(param);
        }
    }

    public interface ISignal<T> : ISignalBody
    {
        void AddListener(Action<T> listener);
        void RemoveListener(Action<T> listener);
        void Dispatch(T param);
    }
}