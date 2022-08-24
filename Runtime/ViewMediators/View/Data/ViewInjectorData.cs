using System;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Data
{
    [Serializable]
    public class ViewInjectorData
    {
        public Object view;
        public bool autoRegister;
        public bool isRegistered;
    }
}