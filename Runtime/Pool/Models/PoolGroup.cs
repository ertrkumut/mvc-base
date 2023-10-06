using System;
using System.Collections.Generic;
using MVC.Runtime.Pool.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.Pool.Models
{
    public class PoolGroup : IPoolGroup
    {
        protected List<string> _poolMap;
        protected Dictionary<string, IItemPool> _pools;
        protected GameObject _mainContainer;
        protected string _groupKey;

        public string Id => _groupKey;
        public GameObject Root => _mainContainer;

        private byte c_generic_pool_type_index_pointer = 1;

        public virtual void Initialize(GameObject root = null)
        {
            _poolMap = new List<string>();
            _pools = new Dictionary<string, IItemPool>();
            _mainContainer = root;
        }

        public void Initialize(GameObject root = null, string groupId = "")
        {
            _groupKey = groupId;
            Initialize(root);
        }

        private GameObject CreatePoolContainer(string key)
        {
            GameObject poolContainer = new GameObject(key);
            poolContainer.transform.parent = _mainContainer.transform;
            poolContainer.transform.localPosition = Vector3.zero;
            poolContainer.transform.localScale = Vector3.one;
            return poolContainer;
        }

        public virtual void RegisterItemAsPool(Type configType, string key, object prefab, int count = 5, bool isExtendable = true, bool getItemEvenUsing = false)
        {
            if (ReferenceEquals(prefab, null))
            {
                Debug.LogError("The item with key " + key + "has no gameobject instance");
            }

            var types = configType.BaseType.GenericTypeArguments;

            CreatePoolWithType(types[c_generic_pool_type_index_pointer], key, prefab, count, isExtendable, getItemEvenUsing);
        }

        private void CreatePoolWithType(
            Type poolType,
            string key,
            object prefab,
            int count = 5,
            bool isExtendable = true,
            bool getItemEvenUsing = false)
        {
            var pool = Activator.CreateInstance(poolType) as IItemPool;
            if (!pool.CheckItemCompatibility(prefab))
            {
                GC.SuppressFinalize(pool);
                pool = null;
                return;
            }

            GameObject poolContainer = CreatePoolContainer("Pool-" + key);
            pool.Initialize(poolContainer, prefab, count, isExtendable, getItemEvenUsing);
            _poolMap.Add(key);
            _pools.Add(key, pool);
        }

        public virtual void RegisterPool(string key, IItemPool pool)
        {
            _poolMap.Add(key);
            _pools.Add(key, pool);
        }

        public virtual T Get<T>(string key) where T : IPoolableItem
        {
            return (T)_pools[key].GetObject();
        }

        public bool ContainsPool(string key)
        {
            return _pools.ContainsKey(key);
        }

        public virtual void ResetAllPools()
        {
            foreach (var poolKey in _poolMap)
            {
                _pools[poolKey].ResetPool();
            }
        }

        public virtual void DestroyPool(string key)
        {
            _poolMap.Remove(key);
            _pools[key].DestroyPool();
            _pools.Remove(key);
        }

        public virtual void DestroyGroup()
        {
            DestroyAllPools();
            Object.Destroy(Root);
        }

        public virtual void DestroyAllPools()
        {
            string[] keys = _poolMap.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                DestroyPool(keys[i]);
                keys[i] = null;
            }

            keys = null;
        }

        public void ResetPool(string key)
        {
            _pools[key].ResetPool();
        }

        public void SetParent(Transform parent)
        {
            _mainContainer.transform.parent = parent;
        }

        public bool CheckAllPoolsReady()
        {
            foreach (var pool in _pools)
                if (!pool.Value.IsReady) 
                    return false;

            return true;
        }
    }
}
