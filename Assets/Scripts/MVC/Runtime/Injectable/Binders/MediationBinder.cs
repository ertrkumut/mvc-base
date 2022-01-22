using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Bind.Bindings;
using MVC.Runtime.Bind.Bindings.Mediation;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Binders
{
    public class MediationBinder : Binder<MediationBinding>
    {
        public override MediationBinding Bind<TKeyType>()
        {
            var viewType = typeof(TKeyType);
            if (!viewType.IsSubclassOf(typeof(IMVCView)))
            {
                Debug.LogError("Binding View require to inherit from IMVCView interface! " + viewType.Name);
                return null;
            }
            
            return base.Bind<TKeyType>();
        }
        
        public override MediationBinding Bind(object key)
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