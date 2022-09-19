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

        private Dictionary<string, ObjectPoolVO> _poolConfigVODict;

        private Dictionary<string, List<IPoolable>> _enabledObjects;
        private Dictionary<string, List<IPoolable>> _disabledObjects;

        private GameObject _container;
        private GameObject _newObj;

        [PostConstruct]
        protected virtual void PostConstruct()
        {
            _poolConfigVODict = new Dictionary<string, ObjectPoolVO>();
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
            var poolVOCount = _data.list.Count;

            for (var ii = 0; ii < poolVOCount; ii++)
            {
                var objectPoolVO = _data.list[ii];
                RegisterPoolObject(objectPoolVO);
            }

            foreach (var objectPool in _poolConfigVODict)
            {
                AutoInstantiateAtStart(objectPool.Value, _container.transform);
            }
        }

        internal void RegisterPoolObject(ObjectPoolVO objectPoolVO)
        {
            if(_poolConfigVODict.ContainsKey(objectPoolVO.Key))
                return;
            
            _poolConfigVODict.Add(objectPoolVO.Key, objectPoolVO);
            
            if(!_disabledObjects.ContainsKey(objectPoolVO.Key))
                _disabledObjects.Add(objectPoolVO.Key, new List<IPoolable>());
            if(!_enabledObjects.ContainsKey(objectPoolVO.Key))
                _enabledObjects.Add(objectPoolVO.Key, new List<IPoolable>());
        }
        
        public void RegisterPoolObject(string key, GameObject prefab, int count = 0)
        {
            if (_poolConfigVODict.ContainsKey(key))
            {
                Debug.LogError("There is already pool config data for key: " + key);
                MVCConsole.LogError(ConsoleLogType.Pool, "There is already pool config data for key: " + key);
                return;
            }
            
            var objectPoolVO = new ObjectPoolVO
            {
                Key = key,
                Count = count,
                Prefab = prefab
            };
            
            RegisterPoolObject(objectPoolVO);
        }

        public void UnRegisterPoolObject(string key)
        {
            if (!_poolConfigVODict.ContainsKey(key))
            {
                Debug.LogError("There is no pool data for: " + key);
                MVCConsole.LogError(ConsoleLogType.Pool, "There is no pool data for: " + key);
                return;
            }

            _poolConfigVODict.Remove(key);
        }

        protected void AutoInstantiateAtStart(ObjectPoolVO poolVO, Transform parent = null)
        {
            var key = poolVO.Key;

            if (poolVO.Prefab == null)
            {
                MVCConsole.LogError(ConsoleLogType.Pool,
                    "Pool Object prefab can not be null! \n pool-key: " + key);
                return;
            }
            
            if(poolVO.Prefab.GetComponent<IPoolable>() == null)
            {
                MVCConsole.LogError(ConsoleLogType.Pool,
                    "Pool Object prefab must be inherited from IPoolable interface! \n pool-key: " + key);
                return;
            }
            
            for(var ii = 0; ii < poolVO.Count; ii++)
            {
                var poolableObject = Object.Instantiate(poolVO.Prefab, parent).GetComponent<IPoolable>();
                _disabledObjects[key].Add(poolableObject);
                poolableObject.transform.gameObject.SetActive(false);
            }
        }
        
        public virtual PoolType Get<PoolType>(string key, Transform parent = null)
            where PoolType : IPoolable
        {
            if (!_poolConfigVODict.ContainsKey(key))
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
            var configVO = GetConfigVOByPoolType(poolType);
            if (configVO == null)
            {
                Debug.LogError("Pool Config Data couldn't found. PoolType: " + poolType, poolItem.transform.gameObject);
                MVCConsole.LogError(ConsoleLogType.Pool, "Pool Config Data couldn't found. PoolType: " + poolType);
                return;
            }
            
            var key = configVO.Key;
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
            var configVO = _poolConfigVODict[key];
            
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
                availableItem = Object.Instantiate(configVO.Prefab, itemParent).GetComponent<IPoolable>();
            }
            
            if(!_enabledObjects.ContainsKey(key))
                _enabledObjects.Add(key, new List<IPoolable>());

            return availableItem;
        }

        protected ObjectPoolVO GetConfigVOByPoolType(Type poolType)
        {
            foreach (var objectPool in _poolConfigVODict)
            {
                var poolableType = objectPool.Value.Prefab.GetComponent<IPoolable>().GetType();
                if (poolableType == poolType)
                    return objectPool.Value;
            }
            
            return null;
        }

        protected ObjectPoolVO GetConfigVOByPrefab(IPoolable poolable)
        {
            foreach (var objectPool in _poolConfigVODict)
            {
                var poolableType = objectPool.Value.Prefab.GetComponent<IPoolable>();
                if (poolableType == poolable)
                    return objectPool.Value;
            }
            
            return null;
        }
    }
}