using UnityEngine;

namespace MVC.Runtime.Pool
{
    public interface IObjectPoolModel
    {
        internal void Initialize();
        
        void RegisterPoolObject(string key, GameObject prefab, int count = 0);
        void UnRegisterPoolObject(string key);

        PoolType Get<PoolType>(string key, Transform parent = null)
            where PoolType : IPoolable;

        ObjectPoolVO GetConfigVoByPoolKey(string key);
        
        void Release(IPoolable poolItem);
    }
}