using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
    public interface IItemPool
    {
        bool IsReady { get; }
        int ReadyCount { get; }
        int UsingCount { get; }
        int TotalItemCount { get; }

        bool CheckItemCompatibility(object prefab);
        void Initialize(GameObject container, object prefab, int count, bool isExtendable = true, bool getItemEvenUsing = false);
        void ResetPool();
        object GetObject(Transform parent);
        void ReturnToPool<TU>(TU item) where TU : class, IPoolableItem;
        void DestroyPool();

    }

    public interface IItemPool<T> : IItemPool where T : class, IPoolableItem
    {
    }
}
