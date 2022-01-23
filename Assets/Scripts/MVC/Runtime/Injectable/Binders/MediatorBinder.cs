using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Bind.Bindings.Mediator;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Binders
{
    public class MediatorBinder : Binder<MediatorBinding>
    {
        public new virtual MediatorBinding Bind<TKeyType>()
            where TKeyType : IMVCView
        {
            return base.Bind<TKeyType>();
        }
        
        public override MediatorBinding Bind(object key)
        {
            var viewType = key.GetType();
            if (!typeof(IMVCView).IsAssignableFrom(viewType))
            {
                Debug.LogError("Binding View require to inherit from IMVCView interface! " + viewType.Name);
                return null;
            }
            
            return base.Bind(key);
        }
    }
}