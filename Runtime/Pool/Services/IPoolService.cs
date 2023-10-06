
using MVC.Runtime.Pool.Entities;

namespace MVC.Runtime.Pool.Services
{
	public interface IPoolService
	{
		void Initialize();
		void CreateGroup(string groupConfigKey);
		bool DestroyGroup(string groupConfigKey);
		bool DestroyGroup(int index);
		bool CheckPoolServiceReady();

		T GetItem<T>(string groupKey, string itemKey) where T : IPoolableItem;
		T GetItem<T>(int groupIndex, string itemKey) where T : IPoolableItem;
		T GetItem<T>(string itemKey) where T : IPoolableItem;
		T SeekItem<T>(string itemKey) where T : IPoolableItem;
	}
}