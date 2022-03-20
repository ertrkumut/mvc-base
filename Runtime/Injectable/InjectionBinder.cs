﻿using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Attributes;
using MVC.Runtime.Bind.Bindings.Pool;
using MVC.Runtime.Root;
using UnityEngine;

namespace MVC.Runtime.Injectable
{
    [HideFromModelViewer]
    public class InjectionBinder
    {
        protected Dictionary<Type, List<InjectionBinding>> _container;

        protected BindingPoolController _bindingPoolController;
        
        public InjectionBinder()
        {
            _container = new Dictionary<Type, List<InjectionBinding>>();
            _bindingPoolController = RootsManager.Instance.bindingPoolController;
        }

        #region Bind

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
                _container.Add(injectionType, new List<InjectionBinding>());

            var injectionBinding = _bindingPoolController.GetAvailableBinding<InjectionBinding>();
            injectionBinding.Name = name;
            injectionBinding.SetValue(instance);
            injectionBinding.SetKey(injectionType);
            
            _container[injectionType].Add(injectionBinding);    
        }

        #endregion

        #region UnBind

        public virtual void UnBind<TBindingType>(string name = "")
            where TBindingType : new()
        {
            var hasBindingExist = HasInstanceExist<TBindingType>(name);
            if (!hasBindingExist)
                return;

            UnBind(typeof(TBindingType), name);
        }

        public virtual void UnBindAll()
        {
            foreach (var injectionBindingList in _container)
            {
                var injectionList = injectionBindingList.Value;
                for (var ii = 0; ii < injectionList.Count; ii++)
                {
                    var injectionBinding = injectionList[0];
                    var key = injectionBinding.Key is Type
                        ? injectionBinding.Key as Type
                        : injectionBinding.Key.GetType();

                    UnBind(key, injectionBinding.Name);
                }
            }
        }

        protected void UnBind(Type key, string name = "")
        {
            var injectionBinding = GetInjectionBinding(key, name);
            _container[key].Remove(injectionBinding);
            _bindingPoolController.ReturnBindingToPool(injectionBinding);
            
            Debug.LogWarning("UnBind: " + key.Name + "- name: " + name);
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
                _container.Add(injectionType, new List<InjectionBinding>());

            var injectionData = _bindingPoolController.GetAvailableBinding<InjectionBinding>();
            injectionData.Name = name;
            injectionData.SetValue(instance);
            injectionData.SetKey(injectionType);
            
            _container[injectionType].Add(injectionData);

            return instance;
        }

        private TAbstract CreateInstance<TAbstract, TConcrete>(string name = "")
            where TConcrete : TAbstract, new()
        {
            var instance = new TConcrete();
            var injectionType = typeof(TAbstract);
            
            if(!_container.ContainsKey(injectionType))
                _container.Add(injectionType, new List<InjectionBinding>());

            var injectionData = _bindingPoolController.GetAvailableBinding<InjectionBinding>();
            injectionData.Name = name;
            injectionData.SetValue(instance);
            injectionData.SetKey(injectionType);
            
            _container[injectionType].Add(injectionData);
            return instance;
        }

        #endregion

        public List<InjectionBinding> GetInjectedInstances()
        {
            return _container.Values
                .ToList()
                .SelectMany(list => list)
                .ToList()
                .Select(x => x)
                .ToList();
        }

        internal InjectionBinding GetInjectionBinding(Type key, string name = "")
        {
            var injectionBinding = _container[key].FirstOrDefault(x => x.Name == name);
            return injectionBinding;
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