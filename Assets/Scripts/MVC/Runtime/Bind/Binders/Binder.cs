using System.Collections.Generic;
using MVC.Runtime.Bind.Bindings;
using MVC.Runtime.Bind.Bindings.Pool;
using MVC.Runtime.Root;
using UnityEngine;

namespace MVC.Runtime.Bind.Binders
{
    public class Binder<TBindingType> : IBinder<TBindingType>
        where TBindingType : IBinding, new()
    {
        protected BindingPoolController _bindingPoolController;
        protected Dictionary<object, TBindingType> _bindings;

        public Binder()
        {
            _bindings = new Dictionary<object, TBindingType>();
            _bindingPoolController = RootsManager.Instance.bindingPoolController;
        }

        #region Bind

        public virtual TBindingType Bind<TKeyType>()
        {
            var keyType = typeof(TKeyType);
            if(IsBindingExist(keyType))
            {
                Debug.LogWarning("Binding already exist! keyType: " + keyType.Name);
                return default;
            }

            var binding = (TBindingType) _bindingPoolController.GetAvailableBinding(typeof(TBindingType));
            binding.SetKey(keyType);
            _bindings.Add(keyType, binding);
            return binding;
        }

        public virtual TBindingType Bind(object key)
        {
            if(IsBindingExist(key))
            {
                Debug.LogWarning("Binding already exist! keyType: " + key);
                return default;
            }

            var binding = (TBindingType) _bindingPoolController.GetAvailableBinding(typeof(TBindingType));
            binding.SetKey(key);
            return binding;
        }

        #endregion

        #region UnBind

        public virtual void UnBind(object key)
        {
            var binding = GetBinding(key);
            if (binding == null)
                return;
            
            _bindingPoolController.ReturnBindingToPool(binding);
        }

        #endregion
        
        #region GetBinding

        public TBindingType GetBinding(object key)
        {
            return !IsBindingExist(key) ? default : _bindings[key];
        }

        public TBindingType GetBinding<TKeyType>()
        {
            var keyType = typeof(TKeyType);

            return !IsBindingExist(keyType) ? default : _bindings[keyType];
        }

        #endregion
        
        public bool IsBindingExist(object key)
        {
            return _bindings.ContainsKey(key);
        }
    }
}