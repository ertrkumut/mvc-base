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
        public override void InjectionBindings()
        {
            base.InjectionBindings();

            InjectionBinderCrossContext.Bind<IPoolService, PoolService>();
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

            // _poolSignals.CreatePoolGroup.Dispatch("CombatKingPool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatTowerPool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatUnitPool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatProjectilePool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatCommonParticleFXPool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatKingEffectPool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatAreaEffectPool);
            // _poolSignals.CreatePoolGroup.Dispatch("CombatEffectFXPool);
            // _poolSignals.CreatePoolGroup.Dispatch("AddressableTestPool);
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}