using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Runtime.Injectable
{
    public class InjectionBinder
    {
        protected Dictionary<Type, List<InjectionData>> _container;

        public InjectionBinder()
        {
            _container = new Dictionary<Type, List<InjectionData>>();
        }

        #region BindCrossContextSingletonSafely

        public TBindingType BindCrossContextSingletonSafely<TBindingType>(string name = "")
            where TBindingType : new()
        {
            return GetOrCreateInstance<TBindingType>(name);
        }

        public TAbstract BindCrossContextSingletonSafely<TAbstract, TConcrete>()
            where TConcrete : TAbstract, new()
        {
            return GetOrCreateInstance<TAbstract, TConcrete>();
        }

        #endregion

        protected TBindingType GetInstance<TBindingType>(string name)
        {
            var bindingType = typeof(TBindingType);
            var injectionData = _container[bindingType].FirstOrDefault(x => x.name == name);
            return injectionData != null ? (TBindingType) injectionData.value : default;
        }

        public object GetInstance(Type instanceType, string name = "")
        {
            if (!_container.ContainsKey(instanceType))
                return null;
            
            var values = _container[instanceType];
            var injectionData = values.FirstOrDefault(x => x.name == name);
            return injectionData == null ? null : injectionData.value;
        }

        #region GetOrCreateInstance

        protected TBindingType GetOrCreateInstance<TBindingType>(string name = "")
            where TBindingType : new()
        {
            var bindingType = typeof(TBindingType);
            var hasInstanceExist = HasInstanceExist<TBindingType>();

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

            _container[injectionType].Add(new InjectionData
            {
                name = name,
                type = injectionType,
                value = instance
            });

            return instance;
        }

        private TAbstract CreateInstance<TAbstract, TConcrete>(string name = "")
            where TConcrete : TAbstract, new()
        {
            var instance = new TConcrete();
            var injectionType = typeof(TAbstract);
            
            if(!_container.ContainsKey(injectionType))
                _container.Add(injectionType, new List<InjectionData>());

            _container[injectionType].Add(new InjectionData
            {
                name = name,
                type = injectionType,
                value = instance
            });
            return instance;
        }

        #endregion

        internal List<object> GetInjectedInstances()
        {
            return _container.Values
                .ToList()
                .SelectMany(list => list)
                .ToList()
                .Select(x => x.value)
                .ToList();
        }
        
        protected bool HasInstanceExist<TBindingType>()
        {
            var bindingType = typeof(TBindingType);
            return _container.ContainsKey(bindingType);
        }
    }
}