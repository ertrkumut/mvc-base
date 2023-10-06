using MVC.Runtime.Contexts;
using MVC.Runtime.Pool.Models;
using MVC.Runtime.Pool.Services;

namespace MVC.Runtime.Pool.Root
{
    public class PoolContext : Context
    {
        private IPoolService _service;
        public override void InjectionBindings()
        {
            base.InjectionBindings();

            _service = InjectionBinderCrossContext.Bind<IPoolService, PoolService>();
            InjectionBinder.Bind<IPoolModel, PoolModel>();
            InjectionBinder.Bind<IPoolConfigModel, PoolConfigModel>();
        }

        public override void Setup()
        {
            base.Setup();
            _service.Initialize();
        }
    }
}