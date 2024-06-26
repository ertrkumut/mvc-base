using System;
using UnityEngine;

namespace MVC.Runtime.Pool.Entities
{
    public class PoolableItem : MonoBehaviour, IPoolableItem
    {
        public Action<IPoolableItem> ReturnToPoolAction { get; set; }

        public virtual void SetActive()
        {
            gameObject.SetActive(true);
        }

        public virtual void Dismiss()
        {
            ReturnToPoolAction?.Invoke(this);
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