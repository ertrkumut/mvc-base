using MVC.Runtime.Root;
using UnityEngine;

namespace MVC.Runtime.Pool.Root
{
	public class PoolRoot : Root<PoolContext>
	{
		protected override void BeforeCreateContext()
		{
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
}
