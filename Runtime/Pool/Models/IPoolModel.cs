using MVC.Runtime.Pool.Data.UnityObjects;
using MVC.Runtime.Pool.Entities;
using UnityEngine;

namespace MVC.Runtime.Pool.Models
{
	public interface IPoolModel
	{
		void Initialize(GameObject root = null);
		void CreateGroup(CD_PoolGroupBase poolConfig);
		bool DestroyGroup(string key);
		bool DestroyGroup(int index);
		void ResetGroup(string key);

		T GetItem<T>(string groupKey, string itemKey, Transform parent) where T : IPoolableItem;
		T GetItem<T>(int groupIndex, string itemKey, Transform parent) where T : IPoolableItem;
		T GetItem<T>(string itemKey, Transform parent) where T : IPoolableItem;
		T SeekItem<T>(string itemKey, Transform parent) where T : IPoolableItem;
		
		bool CheckAllPoolGroupsReady();
	}
}