using System;
using MVC.Runtime.Contexts;
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
                Debug.LogError("There is no Context \nviewType: " + view.GetType().Name);
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
                mediator = mediatorBinder.GetMediatorFromPool(mediatorType);

            var injectionResult = viewContext.TryToInjectMediator(mediator, view);
            if (injectionResult)
            {
                var injectedMediatorData = mediatorBinder.GetOrCreateInjectedMediatorData(view);
                injectedMediatorData.viewInjectorComponent.ViewInjectionCompleted(view);
                injectedMediatorData.mediator = mediator;
            }
            return injectionResult;
        }

        public static void RemoveRegistration(this IView view)
        {
            var viewContext = view.FindViewContext();
            if (viewContext == null)
            {
                Debug.LogError("There is no Context \nviewType: " + view.GetType().Name);
                return;
            }
            
            var mediatorBinder = viewContext.MediatorBinder;
            var injectedMediatorData = mediatorBinder.GetOrCreateInjectedMediatorData(view);
            if (injectedMediatorData.mediator == null)
                return;
            
            var viewInjectorComponent = injectedMediatorData.viewInjectorComponent;

            var viewInjectorData = viewInjectorComponent.GetViewInjectorData(view);
            if (!viewInjectorData.isInjected)
                return;

            var mediator = injectedMediatorData.mediator;
            mediator.OnRemove();

            injectedMediatorData.mediator = null;
            viewInjectorData.isInjected = false;

            if (mediator is Object mediatorObject)
                Object.Destroy(mediatorObject as Component);
            else
                mediatorBinder.SendMediatorToPool(mediator);
        }
        
        internal static IContext FindViewContext(this IView view)
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

        internal static bool IsMediatorMono(this IMediator mediator)
        {
            return mediator is Object;
        }
    }
}