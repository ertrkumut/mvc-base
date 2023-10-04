using MVC.Runtime.Controller;
using MVC.Runtime.Injectable.Attributes;

namespace MVC.Runtime.PoolDeprecated.Controllers
{
    public class InitializePoolCommand : Command
    {
        [Inject] private IObjectPoolModel _objectPoolModel { get; set; }
        
        public override void Execute()
        {
            _objectPoolModel.Initialize();
        }
    }
}