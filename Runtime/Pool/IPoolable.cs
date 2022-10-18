using UnityEngine;

namespace MVC.Runtime.Pool
{
    public interface IPoolable
    {
        Transform transform { get;  }
        
        string PoolKey { get; set; }
        
        void OnGetFromPool();
        void OnReturnToPool();
    }
}