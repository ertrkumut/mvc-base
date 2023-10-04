using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Pool.Entities;
using MVC.Runtime.Pool.Models;
using MVC.Runtime.Pool.Root;
using UnityEngine;

namespace MVC.Runtime.Pool.Services
{
    public class PoolService : IPoolService
    {
        [Inject] private IPoolModel _poolModel;
        [Inject] private IPoolConfigModel _poolConfigModel;
        [Inject(nameof(PoolContext))] private GameObject _root;
        
        [PostConstruct]
        private void OnPostConstruct()
        {
            _poolModel.Initialize(_root);
            Build();
        }
        private void Build()
        {
            PoolRootAdapter adapter = _root.GetComponent<PoolRootAdapter>();
            if (adapter != null)
            {
                foreach (var config in adapter.PoolConfigs)
                {
                    _poolConfigModel.AddConfig(config);

                    if (config.AutoInit)
                    {
                        if (!ReferenceEquals(config, null))
                            _poolModel.CreateGroup(config);
                    }
                }
            }
        }
        public void CreateGroup(string groupConfigKey)
        {
            var config = _poolConfigModel.GetConfigByKey(groupConfigKey);
            if (!ReferenceEquals(config, null))
                _poolModel.CreateGroup(config);
        }
        public bool DestroyGroup(string groupConfigKey)
        {
            return _poolModel.DestroyGroup(groupConfigKey);
        }

        public bool DestroyGroup(int index)
        {
            return _poolModel.DestroyGroup(index);
        }

        public T GetItem<T>(string groupKey, string itemKey) where T : IPoolableItem
        {
            return _poolModel.GetItem<T>(groupKey, itemKey);
        }

        public T GetItem<T>(int groupIndex, string itemKey) where T : IPoolableItem
        {
            return _poolModel.GetItem<T>(groupIndex, itemKey);
        }
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemKey"></param>
        /// <returns>Returns an item from fist added pool group</returns>
        public T GetItem<T>(string itemKey) where T : IPoolableItem
        {
            return _poolModel.GetItem<T>(itemKey);
        }

        public T SeekItem<T>(string itemKey) where T : IPoolableItem
        {
            return _poolModel.SeekItem<T>(itemKey);
        }
    }
}