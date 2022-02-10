using System;
using MVC.Runtime.Contexts;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Injectable.Utils;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Mediator;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.Utils
{
    public static class ViewExtensions
    {
        public static bool InjectView(this IView view)
        {
            var viewContext = view.FindViewContext();
            if (viewContext == null)
            {
                Debug.LogError("There is no Context");
                return false;
            }

            var mediatorBinder = viewContext.MediatorBinder;
            var binding = mediatorBinder.GetBinding(view.GetType());
            if (binding == null)
            {
                Debug.LogError("There is no view binding! " + view.GetType());
                return false;
            }
            
            var mediatorType = binding.Value as Type;
            var mediatorIsMono = mediatorType.IsSubclassOf(typeof(Object));

            IMediator mediator;
            
            if(mediatorIsMono)
                mediator = view.gameObject.AddComponent(mediatorType) as IMediator;
            else
                mediator = (IMediator) Activator.CreateInstance(mediatorType);

            var injectionResult = viewContext.TryToInjectMediator(mediator, view);
            if (injectionResult)
            {
                var viewInjectorComponent = view.transform.GetComponent<ViewInjectorComponent>();
                viewInjectorComponent.ViewInjectionCompleted(view);
            }
            return injectionResult;
        }

        public static IContext FindViewContext(this IView view)
        {
            var parent = view.transform.parent;
            if (parent == null)
                return null;

            IContextRoot contextRoot = null;

            while (contextRoot == null)
            {
                contextRoot = parent.GetComponent<IContextRoot>();
            }
            
            return contextRoot == null ? null : contextRoot.GetContext();
        }
    }
}