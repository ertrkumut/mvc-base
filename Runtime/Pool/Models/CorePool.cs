using System.Collections.Generic;
using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
    public abstract class CorePool<T> : IItemPool<T> where T : class, IPoolableItem
    {
        protected LinkedList<T> _readyItems;
        protected LinkedList<T> _usingItems;
        protected GameObject _container;
        protected int _initialCount;
        protected bool _isExtendable;
        protected bool _getItemEvenUsing;
        protected bool _isDestroyed = false;
        
        private bool _isReady = false;
        
        public int ReadyCount
        {
            get
            {
                return _readyItems.Count;
            }
        }

        public int UsingCount
        {
            get
            {
                return _usingItems.Count;
            }
        }

        public int TotalItemCount
        {
            get
            {
                return _readyItems.Count + _usingItems.Count;
            }
        }
        
        public bool IsReady => _isReady;

        protected abstract void CreateItem();

        public virtual void Initialize(GameObject container, object asset, int count, bool extendable = true, bool getItemEvenUsing = false)
        {
            _container = container;
            _initialCount = count;
            _isExtendable = extendable;
            _getItemEvenUsing = getItemEvenUsing;

            _readyItems = new LinkedList<T>();
            _usingItems = new LinkedList<T>();
        }

        protected void InternalInitialize()
        {
            for (int i = 0; i < _initialCount; i++)
            {
                CreateItem();
            }
            
            _isReady = true;
        }

        protected virtual void PrepareItem(T item)
        {
            item.transform.gameObject.SetActive(false);
            try
            {
                item.OnInitialized();
                item.ReturnPoolCallback = ReturnToPool;
                _readyItems.AddFirst(item);
            }
            catch (System.Exception e)
            {
                var errString =
                    "<b><color=#ff6694> Error on Creating Poool Item!</color></b>" +
                    "\n<b><color=#ff6694>Pool Container=></color></b>" + _container.name +
                    "\n<color=#ffc966>Items removed due to will not be used properly</color>" +
                    "\n<color=#ff6694>Error Message:</color>" + e.Message;

                Debug.LogError(errString);
                Object.Destroy(item.transform.gameObject);
            }
        }

        protected virtual T GetItem()
        {
            if (_isDestroyed)
            {
                Debug.LogWarning("The Item you are trying to get, is already destroyed.");
                return null;
            }

            if (_readyItems.Count == 0)
            {
                if (_isExtendable)
                {
                    CreateItem();
                }
                else if (_getItemEvenUsing)
                {
                    T usingItem = _usingItems.First.Value;
                    usingItem.OnGetFromPool();

                    _usingItems.RemoveFirst();
                    _usingItems.AddLast(usingItem);
                    return usingItem;
                }
                else
                {
                    Debug.LogWarning("There is not enough ready item to get from pool.");
                    return null;
                }
            }

            T item = _readyItems.First.Value;
            item.OnGetFromPool();

            _readyItems.RemoveFirst();
            _usingItems.AddLast(item);

            return item;
        }

        public virtual object GetObject()
        {
            return GetItem();
        }

        public void ReturnToPool<TU>(TU item) where TU : class, IPoolableItem
        {
            _usingItems.Remove(item as T);
            _readyItems.AddLast(item as T);
            item.transform.parent = _container.transform;
            item.transform.gameObject.SetActive(false);
            item.OnReturnToPool();
        }

        public void ResetPool()
        {
            foreach (var item in _usingItems)
            {
                ReturnToPool(item);
            }
        }

        public virtual void DestroyPool()
        {
            _isDestroyed = true;

            while (_readyItems.Count > 0)
            {
                T item = _readyItems.First.Value;

                try
                {
                    Object.Destroy(item.transform.gameObject);
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("Item already destroyed from editor, now destroyed from pool");
                }

                _readyItems.RemoveFirst();
            }

            while (_usingItems.Count > 0)
            {
                T item = _usingItems.First.Value;
                Object.Destroy(item.transform.gameObject);
                _usingItems.RemoveFirst();
            }

            Object.Destroy(_container);
        }

        public virtual bool CheckItemCompatibility(object prefab)
        {
            return true;
        }
    }
}
