using System;
using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
    public interface IPoolGroup
    {
        public  string Id { get; }
        public GameObject Root { get; }

        void Initialize(GameObject root = null);
        void Initialize(GameObject root = null, string groupId = "");
        void RegisterItemAsPool(Type configType, string key, object prefab, int count = 10, bool isExtendable = true, bool getItemEvenUsing = false);
        void RegisterPool(string key, IItemPool pool);
        void DestroyGroup();
        void DestroyPool(string key);
        void DestroyAllPools();
        bool ContainsPool(string key);

        public void ResetAllPools();
        public void ResetPool(string key);
        public T Get<T>(string key) where T : IPoolableItem;
        void SetParent(Transform parent);
        
        bool CheckAllPoolsReady();
    }
}