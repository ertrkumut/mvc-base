using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Bind.Bindings.Mediator;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Binders
{
    public class MediatorBinder : Binder<MediatorBinding>
    {
        public override MediatorBinding Bind<TKeyType>()
        {
            var viewType = typeof(TKeyType);
            if (!viewType.IsSubclassOf(typeof(IMVCView)))
            {
                Debug.LogError("Binding View require to inherit from IMVCView interface! " + viewType.Name);
                return null;
            }
            
            return base.Bind<TKeyType>();
        }
        
        public override MediatorBinding Bind(object key)
        {
            var viewType = key.GetType();
            if (!viewType.IsSubclassOf(typeof(IMVCView)))
            {
                Debug.LogError("Binding View require to inherit from IMVCView interface! " + viewType.Name);
                return null;
            }
            
            return base.Bind(key);
        }
    }
}