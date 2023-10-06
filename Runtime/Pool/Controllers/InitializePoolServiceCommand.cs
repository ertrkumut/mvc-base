using MVC.Runtime.Controller;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Pool.Services;
using MVC.Runtime.Provider.Coroutine;
using UnityEngine;

namespace MVC.Runtime.Pool.Controllers
{
    public class InitializePoolServiceCommand : Command
    {
        [Inject] private ICoroutineProvider _coroutineProvider;
        [Inject] private IPoolService _poolService;

        private GameObject _currentEventSystem;

        public override void Execute()
        {
            _poolService.Initialize();
        }

    }
}