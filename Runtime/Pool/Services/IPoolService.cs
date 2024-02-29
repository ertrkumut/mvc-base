using MVC.Runtime.Pool.Data.UnityObjects;
using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Services
{
	public interface IPoolService
	{
		void Initialize();
		void CreateGroup(string groupConfigKey);
		void CreateGroup(CD_PoolGroupBase newConfig);
		bool DestroyGroup(string groupConfigKey);
		bool DestroyGroup(int index);
		bool CheckPoolServiceReady();
		void ResetGroup(string key);
		T GetItem<T>(string groupKey, string itemKey, Transform parent = null) where T : IPoolableItem;
		T GetItem<T>(int groupIndex, string itemKey, Transform parent = null) where T : IPoolableItem;
		T GetItem<T>(string itemKey, Transform parent = null) where T : IPoolableItem;
		T SeekItem<T>(string itemKey, Transform parent = null) where T : IPoolableItem;
	}
}