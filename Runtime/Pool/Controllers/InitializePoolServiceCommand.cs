using MVC.Runtime.Controller;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Pool.Services;

namespace MVC.Runtime.Pool.Controllers
{
    public class InitializePoolServiceCommand : Command
    {
        [Inject] protected IPoolService _poolService;

        public override void Execute()
        {
            _poolService.Initialize();
        }

    }
}