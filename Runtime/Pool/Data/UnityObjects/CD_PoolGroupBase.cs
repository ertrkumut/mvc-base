using System.Collections;
using System.Collections.Generic;
using MVC.Runtime.Pool.Data.ValueObjects;
using MVC.Runtime.Pool.Models;
using UnityEngine;

namespace MVC.Runtime.Pool.Data.UnityObjects
{
    [AddComponentMenu("")]
    public abstract class CD_PoolGroupBase<TVO, POOL_TYPE> : CD_PoolGroupBase where TVO : PoolItemBaseVO where POOL_TYPE : IItemPool
    {
        protected abstract List<TVO> _items { get; }

        public override int Count => _items.Count;

        public override PoolItemBaseVO this[int index]
        {
            get => _items[index];
            set => _items[index] = (TVO)value;
        }

        public override void Add(PoolItemBaseVO item)
        {
            _items.Add((TVO)item);
        }

        public override void Clear()
        {
            _items.Clear();
        }

        public override bool Contains(PoolItemBaseVO item)
        {
            return _items.Contains((TVO)item);
        }

        public override void CopyTo(PoolItemBaseVO[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
            {
                _items.Add((TVO)array[i]);
            }
        }

        public override bool Remove(PoolItemBaseVO item)
        {
            return _items.Remove((TVO)item);
        }

        public override int IndexOf(PoolItemBaseVO item)
        {
            return _items.IndexOf((TVO)item);
        }

        public override void Insert(int index, PoolItemBaseVO item)
        {
            _items.Insert(index, (TVO)item);
        }

        public override void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public override IEnumerator<PoolItemBaseVO> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }


    [AddComponentMenu("")]
    public abstract class CD_PoolGroupBase : ScriptableObject, IList<PoolItemBaseVO>
    {
        public bool AutoInit = false;
        public string GroupName = "GroupName";

        public bool IsReadOnly => false;

        public abstract PoolItemBaseVO this[int index] { get; set; }
        public abstract int Count { get; }
        public abstract void Add(PoolItemBaseVO item);
        public abstract void Clear();
        public abstract bool Contains(PoolItemBaseVO item);
        public abstract void CopyTo(PoolItemBaseVO[] array, int arrayIndex);
        public abstract bool Remove(PoolItemBaseVO item);
        public abstract int IndexOf(PoolItemBaseVO item);
        public abstract void Insert(int index, PoolItemBaseVO item);
        public abstract void RemoveAt(int index);
        public abstract IEnumerator<PoolItemBaseVO> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}