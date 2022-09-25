using System;
using System.Collections.Generic;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Pool.UnityObject;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.Pool
{
    public class ObjectPoolModel : IObjectPoolModel
    {
        protected CD_PoolData _data;

        private Dictionary<string, ObjectPoolVO> _poolMap;

        private Dictionary<string, List<IPoolable>> _enabledObjects;
        private Dictionary<string, List<IPoolable>> _disabledObjects;

        private GameObject _container;
        private GameObject _newObj;

        [PostConstruct]
        protected virtual void PostConstruct()
        {
            _poolMap = new Dictionary<string, ObjectPoolVO>();
            _enabledObjects = new Dictionary<string, List<IPoolable>>();
            _disabledObjects = new Dictionary<string, List<IPoolable>>();
            
            _container = new GameObject("[MVC]==> PoolObjects");
            Object.DontDestroyOnLoad(_container);
        }

        void IObjectPoolModel.Initialize()
        {
            _data = Resources.Load<CD_PoolData>("Data/Config/CD_PoolData");
            
            if (_data == null)
            {
                Debug.LogError("There is no CD_PoolData File!");
                MVCConsole.LogError(ConsoleLogType.Pool, "There is no CD_PoolData File!");
                return;
            }
            
            MVCConsole.LogWarning(ConsoleLogType.Pool, "MVC Pool Model Initialized!");
            var poolCount = _data.List.Count;

            for (var ii = 0; ii < poolCount; ii++)
            {
                var objectPool = _data.List[ii];
                RegisterPoolObject(objectPool);

                AutoInstantiateAtStart(objectPool, _container.transform);
            }
        }

        internal void RegisterPoolObject(ObjectPoolVO objectPoolVO)
        {
            if(_poolMap.ContainsKey(objectPoolVO.Key))
                return;
            
            _poolMap.Add(objectPoolVO.Key, objectPoolVO);
            
            if(!_disabledObjects.ContainsKey(objectPoolVO.Key))
                _disabledObjects.Add(objectPoolVO.Key, new List<IPoolable>());
            if(!_enabledObjects.ContainsKey(objectPoolVO.Key))
                _enabledObjects.Add(objectPoolVO.Key, new List<IPoolable>());
        }
        
        public void RegisterPoolObject(string key, GameObject prefab, int count = 0)
        {
            if (_poolMap.ContainsKey(key))
            {
                Debug.LogError("There is already pool data for key: " + key);
                MVCConsole.LogError(ConsoleLogType.Pool, "There is already pool data for key: " + key);
                return;
            }
            
            var objectPool = new ObjectPoolVO
            {
                Key = key,
                Count = count,
                Prefab = prefab
            };
            
            _poolMap.Add(key, objectPool);
        }

        public void UnRegisterPoolObject(string key)
        {
            if (!_poolMap.ContainsKey(key))
            {
                Debug.LogError("There is no pool data for: " + key);
                MVCConsole.LogError(ConsoleLogType.Pool, "There is no pool data for: " + key);
                return;
            }

            _poolMap.Remove(key);
        }

        protected void AutoInstantiateAtStart(ObjectPoolVO pool, Transform parent = null)
        {
            var key = pool.Key;
            if(pool.Prefab.GetComponent<IPoolable>() == null)
            {
                MVCConsole.LogError(ConsoleLogType.Pool,
                    "Pool Object prefab must be inherited from IPoolable interface! \n pool-key: " + key);
                return;
            }
            
            for(var ii = 0; ii < pool.Count; ii++)
            {
                var poolableObject = Object.Instantiate(pool.Prefab, parent).GetComponent<IPoolable>();
                _disabledObjects[key].Add(poolableObject);
                poolableObject.transform.gameObject.SetActive(false);
            }
        }
        
        public virtual PoolType Get<PoolType>(string key, Transform parent = null)
            where PoolType : IPoolable
        {
            if (!_poolMap.ContainsKey(key))
            {
                Debug.LogError("Pool Config Data not found for: " + key + " PoolType: " + typeof(PoolType).Name);
                MVCConsole.LogError(ConsoleLogType.Pool, "Pool Config Data not found for: " + key + " PoolType: " + typeof(PoolType).Name);
                return default;
            }

            var availableItem = (PoolType) GetAvailablePoolItem(key, parent);
            
            _disabledObjects[key].Remove(availableItem);
            _enabledObjects[key].Add(availableItem);

            availableItem.OnGetFromPool();
            MVCConsole.Log(ConsoleLogType.Pool, "Object Taken From Pool key: " + key);
            return availableItem;
        }

        public virtual void Release(IPoolable poolItem)
        {
            var poolType = poolItem.GetType();
            var pool = GetConfigVoByPoolType(poolType);
            if (pool == null)
            {
                Debug.LogError("Pool Config Data couldn't found. PoolType: " + poolType, poolItem.transform.gameObject);
                MVCConsole.LogError(ConsoleLogType.Pool, "Pool Config Data couldn't found. PoolType: " + poolType);
                return;
            }
            
            var key = pool.Key;
            MVCConsole.Log(ConsoleLogType.Pool, "Object Released To Pool key: " + key);
            
            if(!_disabledObjects.ContainsKey(key))
                _disabledObjects.Add(key, new List<IPoolable>());
            if(!_enabledObjects.ContainsKey(key))
                _enabledObjects.Add(key, new List<IPoolable>());
            
            _disabledObjects[key].Add(poolItem);
            _enabledObjects[key].Remove(poolItem);
            
            poolItem.transform.SetParent(_container.transform);
            
            poolItem.OnReturnToPool();
        }

        protected IPoolable GetAvailablePoolItem(string key, Transform parent = null)
        {
            var pool = _poolMap[key];
            
            if(!_disabledObjects.ContainsKey(key))
                _disabledObjects.Add(key, new List<IPoolable>());

            var poolList = _disabledObjects[key];
            var itemParent = parent == null ? _container.transform : parent;
            
            IPoolable availableItem = null;
            if (poolList.Count != 0)
            {
                availableItem = poolList[0];
                availableItem.transform.SetParent(itemParent);
            }
            else
            {
                availableItem = Object.Instantiate(pool.Prefab, itemParent).GetComponent<IPoolable>();
            }
            
            if(!_enabledObjects.ContainsKey(key))
                _enabledObjects.Add(key, new List<IPoolable>());

            return availableItem;
        }

        protected ObjectPoolVO GetConfigVoByPoolType(Type poolType)
        {
            foreach (var objectPool in _poolMap)
            {
                var poolableType = objectPool.Value.Prefab.GetComponent<IPoolable>().GetType();
                if (poolableType == poolType)
                    return objectPool.Value;
            }
            
            return null;
        }

        protected ObjectPoolVO GetConfigVoByPrefab(IPoolable poolable)
        {
            foreach (var objectPool in _poolMap)
            {
                var poolableType = objectPool.Value.Prefab.GetComponent<IPoolable>();
                if (poolableType == poolable)
                    return objectPool.Value;
            }
            
            return null;
        }
    }
}