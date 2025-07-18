﻿using System;

namespace MVC.Runtime.Signals
{
    public class Signal<T1, T2, T3, T4> : SignalBody, ISignal<T1, T2, T3, T4>
    {
        private event Action<T1, T2, T3, T4> _callbackOnce;
        private event Action<T1, T2, T3, T4> _callback;

        public void AddListenerOnce(Action<T1, T2, T3, T4> listener)
        {
            _callbackOnce += listener;
        }

        public void AddListener(Action<T1, T2, T3, T4> listener)
        {
            _callback += listener;
        }

        public void RemoveListener(Action<T1, T2, T3, T4> listener)
        {
            _callback -= listener;
            _callbackOnce -= listener;
        }

        public void Dispatch(T1 param1, T2 param2, T3 param3, T4 param4)
        {
            _callbackOnce?.Invoke(param1, param2, param3, param4);
            _callbackOnce = null;
            
            _internalCallback?.Invoke(this, new []
            {
                param1 as object,
                param2 as object,
                param3 as object,
                param4 as object
            });
            _callback?.Invoke(param1, param2, param3, param4);
        }
    }

    public interface ISignal<T1, T2, T3, T4> : ISignalBody
    {
        void AddListenerOnce(Action<T1, T2, T3, T4> listener);
        void AddListener(Action<T1, T2, T3, T4> listener);
        void RemoveListener(Action<T1, T2, T3, T4> listener);
        void Dispatch(T1 param1, T2 param2, T3 param3, T4 param4);
    }
}