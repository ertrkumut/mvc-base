using System.Collections.Generic;
using MVC.Runtime.Bind.Bindings;
using UnityEngine;

namespace MVC.Runtime.Bind.Binders
{
    public class Binder : IBinder
    {
        protected Dictionary<object, IBinding> _bindings;

        public Binder()
        {
            _bindings = new Dictionary<object, IBinding>();
        }

        #region CreateBinding

        public virtual IBinding Bind<TKeyType>()
        {
            var keyType = typeof(TKeyType);
            if(IsBindingExist(keyType))
            {
                Debug.LogWarning("Binding already exist! keyType: " + keyType.Name);
                return null;
            }

            var binding = new Binding(keyType);
            _bindings.Add(keyType, binding);
            return binding;
        }

        public virtual IBinding Bind(object key)
        {
            if(IsBindingExist(key))
            {
                Debug.LogWarning("Binding already exist! keyType: " + key);
                return null;
            }

            var binding = new Binding(key);
            return binding;
        }

        #endregion

        #region GetBinding

        public IBinding GetBinding(object key)
        {
            return IsBindingExist(key) ? null : _bindings[key];
        }

        public IBinding GetBinding<TKeyType>()
        {
            var keyType = typeof(TKeyType);

            return IsBindingExist(keyType) ? null : _bindings[keyType];
        }

        #endregion
        
        public bool IsBindingExist(object key)
        {
            return _bindings.ContainsKey(key);
        }
    }
}