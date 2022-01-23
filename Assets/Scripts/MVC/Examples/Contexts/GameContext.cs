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
        }

        private void BindViews()
        {
            MediatorBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
        }
    }

    public class TestClass
    {
        
    }
}