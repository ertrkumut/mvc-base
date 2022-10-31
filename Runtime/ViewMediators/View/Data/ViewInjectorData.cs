using System;
using MVC.Runtime.Root;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Data
{
    [Serializable]
    public class ViewInjectorData
    {
        public Object View;
        public bool AutoRegister;
        public bool UseBubbleUp = true;
        public bool InjectableView;
        public RootBase SelectedRoot;
        public bool IsRegistered;
    }
}