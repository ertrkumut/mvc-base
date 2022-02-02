using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Runtime.Injectable
{
    public class InjectionBinder
    {
        protected Dictionary<Type, object> _container;

        public InjectionBinder()
        {
            _container = new Dictionary<Type, object>();
        }

        #region BindCrossContextSingletonSafely

        public TBindingType BindCrossContextSingletonSafely<TBindingType>()
            where TBindingType : new()
        {
            return GetOrCreateInstance<TBindingType>();
        }

        public TAbstract BindCrossContextSingletonSafely<TAbstract, TConcrete>()
            where TConcrete : TAbstract, new()
        {
            return GetOrCreateInstance<TAbstract, TConcrete>();
        }

        #endregion

        protected TBindingType GetInstance<TBindingType>()
        {
            return (TBindingType) _container[typeof(TBindingType)];
        }

        public object GetInstance(Type instanceType)
        {
            return _container.ContainsKey(instanceType) ? _container[instanceType] : null;
        }

        #region GetOrCreateInstance

        protected TBindingType GetOrCreateInstance<TBindingType>()
            where TBindingType : new()
        {
            var bindingType = typeof(TBindingType);
            var hasInstanceExist = HasInstanceExist<TBindingType>();

            TBindingType instance;

            if (!hasInstanceExist)
                instance = CreateInstance<TBindingType>();
            else
                instance = (TBindingType) _container[bindingType];
            
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
                instance = (TAbstract) _container[bindingType];
            
            return instance;
        }

        #endregion

        #region CreateInstance

        private TBindingType CreateInstance<TBindingType>()
            where TBindingType : new()
        {
            var instance = new TBindingType();
            _container.Add(typeof(TBindingType), instance);
            
            return instance;
        }

        private TAbstract CreateInstance<TAbstract, TConcrete>()
            where TConcrete : TAbstract, new()
        {
            var instance = new TConcrete();
            _container.Add(typeof(TAbstract), instance);
            return instance;
        }

        #endregion

        internal List<object> GetInjectedInstances()
        {
            return _container.Values.ToList();
        }
        
        protected bool HasInstanceExist<TBindingType>()
        {
            var bindingType = typeof(TBindingType);
            return _container.ContainsKey(bindingType);
        }
    }
}