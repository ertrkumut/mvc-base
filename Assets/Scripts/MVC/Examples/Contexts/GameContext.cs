using MVC.Examples.Entity;
using MVC.Examples.Models;
using MVC.Examples.Views.Player;
using MVC.Runtime.Contexts;

namespace MVC.Examples.Contexts
{
    public class GameContext : Context
    {
        public override void MapBindings()
        {
            base.MapBindings();

            InjectionBinder.BindCrossContextSingletonSafely<TestClass>();
            
            BindViews();
            BindModels();
        }

        private void BindViews()
        {
            MediatorBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
        }
        
        private void BindModels()
        {
            InjectionBinder.BindCrossContextSingletonSafely<ITestModel, TestModel>();
        }
    }
}