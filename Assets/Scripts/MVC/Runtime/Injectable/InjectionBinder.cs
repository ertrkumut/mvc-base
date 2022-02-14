using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Bind.Bindings.Pool;
using MVC.Runtime.Root;

namespace MVC.Runtime.Injectable
{
    public class InjectionBinder
    {
        protected Dictionary<Type, List<InjectionData>> _container;

        protected BindingPoolController _bindingPoolController;
        
        public InjectionBinder()
        {
            _container = new Dictionary<Type, List<InjectionData>>();
            _bindingPoolController = RootsManager.Instance.bindingPoolController;
        }

        #region BindCrossContextSingletonSafely

        public TBindingType Bind<TBindingType>(string name = "")
            where TBindingType : new()
        {
            return GetOrCreateInstance<TBindingType>(name);
        }

        public TAbstract Bind<TAbstract, TConcrete>()
            where TConcrete : TAbstract, new()
        {
            return GetOrCreateInstance<TAbstract, TConcrete>();
        }

        public void BindInstance(object instance, string name = "")
        {
            var hasInstanceExist = GetInstance(instance.GetType(), name);
            if (hasInstanceExist != null)
                return;
            
            var injectionType = instance.GetType();
            
            if(!_container.ContainsKey(injectionType))
                _container.Add(injectionType, new List<InjectionData>());

            var injectionData = _bindingPoolController.GetAvailableBinding<InjectionData>();
            injectionData.Name = name;
            injectionData.Value = instance;
            injectionData.Key = injectionType;
            
            _container[injectionType].Add(injectionData);    
        }

        #endregion

        #region GetInstance

        protected TBindingType GetInstance<TBindingType>(string name)
        {
            var bindingType = typeof(TBindingType);
            var injectionData = _container[bindingType].FirstOrDefault(x => x.Name == name);
            return injectionData != null ? (TBindingType) injectionData.Value : default;
        }

        public object GetInstance(Type instanceType, string name = "")
        {
            if (!_container.ContainsKey(instanceType))
                return null;
            
            var values = _container[instanceType];
            var injectionData = values.FirstOrDefault(x => x.Name == name);
            return injectionData == null ? null : injectionData.Value;
        }

        #endregion

        #region GetOrCreateInstance

        protected TBindingType GetOrCreateInstance<TBindingType>(string name = "")
            where TBindingType : new()
        {
            var bindingType = typeof(TBindingType);
            var hasInstanceExist = HasInstanceExist<TBindingType>(name);

            TBindingType instance;

            if (!hasInstanceExist)
                instance = CreateInstance<TBindingType>(name);
            else
                instance = (TBindingType) GetInstance(bindingType, name);
            
            return instance;
        }

        protected TAbstract GetOrCreateInstance<TAbstract, TConcrete>()
            where TConcrete : TAbstract, new()
        {
            var bindingType = typeof(TAbstract);
            var hasInstanceExist = HasInstanceExist<TAbstract>();

            TAbstract instance;

            if (!hasInstanceExist)
                instance = CreateInstance<TAbstract, TConcrete>();
            else
                instance = (TAbstract) GetInstance(bindingType);
            
            return instance;
        }

        #endregion

        #region CreateInstance

        private TBindingType CreateInstance<TBindingType>(string name = "")
            where TBindingType : new()
        {
            var instance = new TBindingType();
            var injectionType = typeof(TBindingType);
            
            if(!_container.ContainsKey(injectionType))
                _container.Add(injectionType, new List<InjectionData>());

            var injectionData = _bindingPoolController.GetAvailableBinding<InjectionData>();
            injectionData.Name = name;
            injectionData.Value = instance;
            injectionData.Key = injectionType;
            
            _container[injectionType].Add(injectionData);

            return instance;
        }

        private TAbstract CreateInstance<TAbstract, TConcrete>(string name = "")
            where TConcrete : TAbstract, new()
        {
            var instance = new TConcrete();
            var injectionType = typeof(TAbstract);
            
            if(!_container.ContainsKey(injectionType))
                _container.Add(injectionType, new List<InjectionData>());

            var injectionData = _bindingPoolController.GetAvailableBinding<InjectionData>();
            injectionData.Name = name;
            injectionData.Value = instance;
            injectionData.Key = injectionType;
            
            _container[injectionType].Add(injectionData);
            return instance;
        }

        #endregion

        internal List<object> GetInjectedInstances()
        {
            return _container.Values
                .ToList()
                .SelectMany(list => list)
                .ToList()
                .Select(x => x.Value)
                .ToList();
        }
        
        protected bool HasInstanceExist<TBindingType>(string name = "")
        {
            var bindingType = typeof(TBindingType);
            if (!_container.ContainsKey(bindingType))
                return false;
            
            var instanceList = _container[bindingType];
            return instanceList.FirstOrDefault(x => x.Name == name) != null;
        }
    }
}