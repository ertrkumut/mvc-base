using System.Collections.Generic;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Pool.Data.UnityObjects;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
    public class PoolConfigModel : IPoolConfigModel
    {
        private Dictionary<string, CD_PoolGroupBase> _poolGroupConfigs;
        
        [PostConstruct]
        public void Load()
        {
            _poolGroupConfigs = new Dictionary<string, CD_PoolGroupBase>();
        }

        public void AddConfig(CD_PoolGroupBase config)
        {
            if (config.GroupName == "") config.GroupName = config.name;
            
            if (_poolGroupConfigs.ContainsKey(config.GroupName))
            {
                Debug.LogError("Pool group <" + config.GroupName + "> already added ");
                return;
            }

            _poolGroupConfigs.Add(config.GroupName, config);
        }

        public CD_PoolGroupBase GetConfigByKey(string key)
        {
            if (_poolGroupConfigs.ContainsKey(key))
            {
                return _poolGroupConfigs[key];
            }

            return null;
        }

        public bool ContainsKey(string key)
        {
            return _poolGroupConfigs.ContainsKey(key);
        }
    }
}
