using System;
using System.Collections.Generic;

namespace MVC.Runtime.Bind.Bindings.Pool
{
    public class BindingPoolController
    {
        private Dictionary<Type, List<IBinding>> _pool;

        public BindingPoolController()
        {
            _pool = new Dictionary<Type, List<IBinding>>();
        }
        
        private List<IBinding> GetPoolList(Type bindingType)
        {
            if(!_pool.ContainsKey(bindingType))
                _pool.Add(bindingType, new List<IBinding>());

            return _pool[bindingType];
        }
        
        internal IBinding GetAvailableBinding(Type bindingType)
        {
            var bindingPoolList = GetPoolList(bindingType);

            IBinding availableBinding = null;
            
            if (bindingPoolList.Count == 0)
                availableBinding = (IBinding) Activator.CreateInstance(bindingType);
            else
            {
                availableBinding = bindingPoolList[0];
                bindingPoolList.Remove(availableBinding);
            }

            return availableBinding;
        }
        
        internal TBindingType GetAvailableBinding<TBindingType>()
            where TBindingType : IBinding
        {
            var bindingType = typeof(TBindingType);
            var bindingPoolList = GetPoolList(bindingType);

            TBindingType availableBinding;
            
            if (bindingPoolList.Count == 0)
                availableBinding = (TBindingType) Activator.CreateInstance(bindingType);
            else
            {
                availableBinding = (TBindingType) bindingPoolList[0];
                bindingPoolList.Remove(availableBinding);
            }

            return availableBinding;
        }

        internal void ReturnBindingToPool(IBinding binding)
        {
            var bindingType = binding.GetType();
            var poolList = GetPoolList(bindingType);
            _pool[bindingType].Add(binding);
        }
    }
}