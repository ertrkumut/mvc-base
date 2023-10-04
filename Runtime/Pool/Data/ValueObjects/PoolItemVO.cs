using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MVC.Runtime.Pool.Data.ValueObjects
{
    [Serializable]
    public class PoolItemVO : PoolItemBaseVO
    {
        [Header("Asset Config")]
        public GameObject Prefab;

        public override object Asset => Prefab;
    }

    [Serializable]
    [HideReferenceObjectPicker]
    public abstract class PoolItemBaseVO
    {
        [Header("Identification")]
        public string PoolKey;

        [Header("Pool Config")]
        public int InitialCreateCount = 10;
        public bool IsExtendable = true;
        public bool GetItemEvenUsing = false;

        public abstract object Asset { get; }
    }
}