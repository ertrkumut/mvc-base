using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.Mediator;
using MVC.Runtime.ViewMediators.View;

namespace MVC.Runtime.Injectable.Mediator
{
    public class InjectedMediatorData
    {
        public IView view;
        public IMediator mediator;
        public ViewInjectorComponent viewInjectorComponent;
    }
}