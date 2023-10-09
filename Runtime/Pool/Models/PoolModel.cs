using System;
using System.Collections.Generic;
using MVC.Runtime.Pool.Data.UnityObjects;
using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
    public class PoolModel : IPoolModel
    {
        protected List<string> _keyMap;
        protected Dictionary<string, IPoolGroup> _poolGroupMap;
        protected GameObject _mainContainer;

        public virtual void Initialize(GameObject root)
        {
            _keyMap = new List<string>();
            _poolGroupMap = new Dictionary<string, IPoolGroup>();
            _mainContainer = root;
        }
        public void CreateGroup(CD_PoolGroupBase poolConfig)
        {
            string key = poolConfig.GroupName;
            PoolGroup poolGroup = new PoolGroup();

            if (!TryAddGroup(poolGroup, key))
            {
                GC.SuppressFinalize(poolGroup);
                poolGroup = null;
                return;
            }

            foreach (var element in poolConfig)
            {
                poolGroup.RegisterItemAsPool(
                    poolConfig.GetType(),
                    element.PoolKey,
                    element.Asset,
                    element.InitialCreateCount,
                    element.IsExtendable,
                    element.GetItemEvenUsing
                );
            }
        }

        private bool TryAddGroup(IPoolGroup poolManager, string key)
        {
            if (RegisterGroup(poolManager, key))
            {
                GameObject managerContainer = new GameObject("Manager-" + key);
                managerContainer.transform.parent = _mainContainer.transform;
                managerContainer.transform.localPosition = Vector3.zero;
                managerContainer.transform.localScale = Vector3.one;
                poolManager.Initialize(managerContainer);
                return true;
            }

            return false;
        }
        private bool RegisterGroup(IPoolGroup poolManager, string key)
        {
            if (_poolGroupMap.ContainsKey(key))
            {
                Debug.LogWarning("The key : " + key + " is already added");
                return false;
            }

            _keyMap.Add(key);
            _poolGroupMap.Add(key, poolManager);

            return true;
        }
        
        public bool DestroyGroup(string key)
        {
            if (!_poolGroupMap.ContainsKey(key))
            {
                Debug.LogWarning("There is no pool manager to destroy added with key " + key + "");
                return false;
            }

            _keyMap.Remove(key);
            _poolGroupMap[key].DestroyGroup();
            _poolGroupMap.Remove(key);
            
            return true;
        }
        public bool DestroyGroup(int index)
        {
            return DestroyGroup(_keyMap[index]);
        }
        public virtual T GetItem<T>(string groupKey, string itemKey, Transform parent = null) where T : IPoolableItem
        {
            return (T)_poolGroupMap[groupKey].Get<T>(itemKey, parent);
        }
        public virtual T GetItem<T>(int groupIndex, string itemKey, Transform parent = null) where T : IPoolableItem
        {
            return GetItem<T>(_keyMap[groupIndex], itemKey, parent);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemKey"></param>
        /// <returns>Returns an item from fist added pool group</returns>
        public virtual T GetItem<T>(string itemKey, Transform parent = null) where T : IPoolableItem
        {
            return GetItem<T>(_keyMap[0], itemKey, parent);
        }
        public T SeekItem<T>(string itemKey, Transform parent = null) where T : IPoolableItem
        {
            for (int i = 0; i < _keyMap.Count; i++)
            {
                if (_poolGroupMap[_keyMap[i]].ContainsPool(itemKey))
                {
                    return _poolGroupMap[_keyMap[i]].Get<T>(itemKey, parent);
                }
            }

            return default;
        }
        
        public bool CheckAllPoolGroupsReady()
        {
            foreach (var group in _poolGroupMap)
                if (!group.Value.CheckAllPoolsReady()) 
                    return false;

            return true;
        }
    }
}
