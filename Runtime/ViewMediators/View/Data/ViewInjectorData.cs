using System;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Data
{
    [Serializable]
    public class ViewInjectorData
    {
        public Object view;
        public bool autoInject;
        public bool isInjected;
    }
}