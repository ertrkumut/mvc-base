using System;
using UnityEngine;

namespace MVC.Runtime.Pool.Entities
{
    public interface IPoolableItem
    {
        public Action<IPoolableItem> ReturnToPoolAction { get; set; }
        Transform transform { get; }

        void OnInitialized();
        void OnGetFromPool();
        void OnReturnToPool();
        /// <summary>
        /// use => ReturnToPoolAction?.Invoke(this);
        /// </summary>
        void Dismiss();
    }
}
