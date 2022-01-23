using MVC.Examples.Views.Player;
using MVC.Runtime.Contexts;

namespace MVC.Examples.Contexts
{
    public class GameContext : Context
    {
        public override void MapBindings()
        {
            base.MapBindings();

            BindViews();
        }

        private void BindViews()
        {
            _mediatorBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
        }
    }
}