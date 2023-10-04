using System;
using UnityEngine;

namespace MVC.Runtime.Pool.Entities
{
    public interface IPoolableItem
    {
        public Action<IPoolableItem> ReturnPoolCallback { get; set; }
        Transform transform { get; }
        void SetActive();
        void Dismiss();
        void OnInitialized();
        void OnGetFromPool();
        void OnReturnToPool();
    }
}
