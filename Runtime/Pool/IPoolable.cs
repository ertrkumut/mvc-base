using System;
using UnityEngine;

namespace MVC.Runtime.Pool
{
    public interface IPoolable
    {
        Transform transform { get;  }
        
        string PoolKey { get; set; }
        
        void OnGetFromPool();
        void OnReturnToPool();
        Action<IPoolable> ReturnToPoolAction { get; set; }

        void ReturnToPool()
        {
            ReturnToPoolAction?.Invoke(this);
        }
    }
}