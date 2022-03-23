using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;

namespace MVC.Editor.CodeGenerator.TempViews
{
    internal class TempMediator : IMediator
    {
        [Inject] private TempView _view { get; set; }
        
        public void OnRegister()
        {
            //@Register
        }

        public void OnRemove()
        {
            //@Remove
        }
        
        //@Methods
    }
}