using System.Collections.Generic;
using UnityEngine;

namespace MVC.Runtime.Pool.UnityObject
{
    [CreateAssetMenu(fileName = "CD_PoolData", menuName = "MVC/Pool/CD_PoolData")]
    public class CD_PoolData : ScriptableObject
    {
        public List<ObjectPoolVO> list;
    }
}