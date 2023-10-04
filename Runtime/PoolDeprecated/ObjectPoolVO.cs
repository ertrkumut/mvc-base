using System;
using UnityEngine;

namespace MVC.Runtime.PoolDeprecated
{
    [Serializable]
    public class ObjectPoolVO
    {
        public GameObject Prefab;
        public string Key;
        public int Count;
    }
}