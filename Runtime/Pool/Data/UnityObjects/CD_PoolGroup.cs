using System.Collections.Generic;
using MVC.Runtime.Pool.Data.ValueObjects;
using MVC.Runtime.Pool.Entities;
using MVC.Runtime.Pool.Models;
using UnityEngine;

namespace MVC.Runtime.Pool.Data.UnityObjects
{
    [CreateAssetMenu(menuName = "Data/Config/PoolGroup", order = 30)]
    public class CD_PoolGroup : CD_PoolGroupBase<PoolItemVO, ItemPool<IPoolableItem>>
    {
        [SerializeField] private List<PoolItemVO> _itemList;
        protected override List<PoolItemVO> _items => _itemList;
    }
}