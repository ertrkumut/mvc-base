using MVC.Runtime.Contexts;
using MVC.Runtime.Pool.Models;
using MVC.Runtime.Pool.Services;

namespace MVC.Runtime.Pool.Root
{
    public class PoolContext : Context
    {
        public override void SignalBindings()
        {
            base.SignalBindings();
        }
        private IPoolService _service;
        public override void InjectionBindings()
        {
            base.InjectionBindings();

            _service = InjectionBinderCrossContext.Bind<IPoolService, PoolService>();
            InjectionBinder.Bind<IPoolModel, PoolModel>();
            InjectionBinder.Bind<IPoolConfigModel, PoolConfigModel>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Launch()
        {
            base.Launch();
            _service.Initialize();
        }
    }
}