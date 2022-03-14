using MVC.Runtime.Signals;

namespace MVC.Examples.Signals
{
    public class GameSignals
    {
        public Signal Start = new Signal();
        public Signal<int> IntTestSignal = new Signal<int>();
    }
}