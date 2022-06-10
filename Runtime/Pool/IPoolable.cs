using UnityEngine;

namespace MVC.Runtime.Pool
{
    public interface IPoolable
    {
        Transform transform { get;  }
        
        void OnGetFromPool();
        void OnReturnToPool();
    }
}