using System;
using UnityEngine;

namespace MVC.Runtime.Pool
{
    [Serializable]
    public class ObjectPoolVO
    {
        public GameObject prefab;
        public string key;
        public int count;
    }
}