using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
    public class ItemPool<T> : CorePool<T> where T : class, IPoolableItem
    {
        protected GameObject _itemPrefab;

        public override bool CheckItemCompatibility(object prefab)
        {
            GameObject prefabGo = prefab as GameObject;
            if (ReferenceEquals(prefabGo.GetComponent<IPoolableItem>(), null)) return false;

            return true;
        }

        public override void Initialize(GameObject container, object asset, int count, bool extendable = true, bool getItemEvenUsing = false)
        {
            _itemPrefab = asset as GameObject;
            base.Initialize(container, asset, count, extendable, getItemEvenUsing);
            InternalInitialize();
        }

        protected override void CreateItem()
        {
            T item = GameObject.Instantiate(_itemPrefab.transform, _container.transform).GetComponent<T>();
            PrepareItem(item);
        }

    }
}
