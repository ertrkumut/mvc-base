using MVC.Runtime.Controller;
using MVC.Runtime.Injectable.Attributes;

namespace MVC.Runtime.Pool.Controllers
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