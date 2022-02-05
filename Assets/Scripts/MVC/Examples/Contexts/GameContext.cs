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

            InjectionBinder.Bind<TestClass>("Test");
            // InjectionBinder.Bind<TestClass>();
            
            BindViews();
            BindModels();
        }

        private void BindViews()
        {
            MediatorBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
        }
        
        private void BindModels()
        {
            CrossContextInjectionBinder.Bind<ITestModel, TestModel>();
        }
    }
}