using UnityEngine;

namespace MVC.Runtime.PoolDeprecated
{
    public interface IObjectPoolModel
    {
        internal void Initialize();
        
        void RegisterPoolObject(string key, GameObject prefab, int count = 0);
        void UnRegisterPoolObject(string key);

        PoolType Get<PoolType>(string key, Transform parent = null)
            where PoolType : IPoolable;

        void Return(IPoolable poolItem);

        void ReturnAllObjects();
        
        ObjectPoolVO GetConfigVoByPoolKey(string key);
    }
}