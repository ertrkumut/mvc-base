using MVC.Runtime.Pool.Data.UnityObjects;

namespace MVC.Runtime.Pool.Models
{
    public interface IPoolConfigModel
    {
        public void AddConfig(CD_PoolGroupBase config);
        public CD_PoolGroupBase GetConfigByKey(string key);
        public bool ContainsKey(string key);

    }
}
