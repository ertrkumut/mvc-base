using System.Collections.Generic;
using MVC.Runtime.Pool.Data.UnityObjects;
using UnityEngine;

namespace MVC.Runtime.Pool.Root
{
    public class PoolRootAdapter : MonoBehaviour
    {
        [Header("Pool Data Set")]
        [SerializeField]
        protected List<CD_PoolGroupBase> _pools;
        public List<CD_PoolGroupBase> PoolConfigs => _pools;
    }
}
