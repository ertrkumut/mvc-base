using System;
using System.Collections.Generic;

namespace MVC.Runtime.Bind.Bindings.Pool
{
    public class BindingPoolController
    {
        private Dictionary<Type, List<IBinding>> _pool;

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

        internal void ReturnBindingToPool(IBinding binding)
        {
            var bindingType = binding.GetType();
            var poolList = GetPoolList(bindingType);
            _pool[bindingType].Add(binding);
        }
    }
}