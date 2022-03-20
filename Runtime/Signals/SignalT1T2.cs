using System;

namespace MVC.Runtime.Signals
{
    public class Signal<T1, T2> : SignalBody, ISignal<T1, T2>
    {
        private event Action<T1, T2> callback; 
        
        public void AddListener(Action<T1, T2> listener)
        {
            callback += listener;
        }

        public void RemoveListener(Action<T1, T2> listener)
        {
            callback -= listener;
        }

        public void Dispatch(T1 param1, T2 param2)
        {
            _internalCallback?.Invoke(this, new []
            {
                param1 as object,
                param2 as object
            });
            callback?.Invoke(param1, param2);
        }
    }

    public interface ISignal<T1, T2> : ISignalBody
    {
        void AddListener(Action<T1, T2> listener);
        void RemoveListener(Action<T1, T2> listener);
        void Dispatch(T1 param1, T2 param2);
    }
}