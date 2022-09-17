using System;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Data
{
    [Serializable]
    public class ViewInjectorData
    {
        public Object View;
        public bool AutoRegister;
        public bool IsRegistered;
    }
}