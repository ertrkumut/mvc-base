using System;
using UnityEngine;

namespace MVC.Runtime.Pool.Entities
{
    public class PoolableItem : MonoBehaviour, IPoolableItem
    {
        public Action<IPoolableItem> ReturnPoolCallback { get; set; }
        public Transform transform { get; }

        public virtual void SetActive()
        {
            gameObject.SetActive(true);
        }

        public virtual void Dismiss()
        {
            ReturnPoolCallback?.Invoke(this);
        }

        public virtual void OnInitialized()
        {
        }

        public virtual void OnGetFromPool()
        {
        }

        public virtual void OnReturnToPool()
        {
        }
    }
}