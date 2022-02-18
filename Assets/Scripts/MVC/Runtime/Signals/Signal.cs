using System;

namespace MVC.Runtime.Signals
{
    public class Signal : ISignal
    {
        private event Action callback;
        
        public void AddListener(Action listener)
        {
            callback += listener;
        }

        public void RemoveListener(Action listener)
        {
            callback -= listener;
        }

        public void Dispatch()
        {
            callback?.Invoke();
        }
    }

    public interface ISignal : ISignalBody
    {
        void AddListener(Action listener);
        void RemoveListener(Action listener);
        void Dispatch();
    }
}