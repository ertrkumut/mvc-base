using System;
using MVC.Editor.Console;
using MVC.Runtime.Bind.Bindings;
using MVC.Runtime.Console;
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
            var context = view.FindViewContext();
            if (context == null)
            {
                Debug.LogError("There is no Context \nviewType: " + view.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Injection, "There is no Context \nviewType: " + view.GetType().Name);
                return false;
            }

            var viewBindingData = context.GetBindingData(view);
            if (viewBindingData.Equals(default))
            {
                Debug.LogError("There is no view binding! " + view.GetType());
                MVCConsole.LogError(ConsoleLogType.Injection, "There is no view binding! " + view.GetType());
                return false;
            }
            else if (viewBindingData.Context == null)
            {
                Debug.LogError("There is no Context \nviewType: " + view.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Injection, "There is no Context \nviewType: " + view.GetType().Name);
                return false;
            }
            
            var mediationBinder = viewBindingData.Context.MediationBinder;
            var mediatorType = viewBindingData.Binding.Value as Type;
            var mediatorIsMono = mediatorType.IsSubclassOf(typeof(Object));

            IMediator mediator;
            
            if(mediatorIsMono)
                mediator = view.gameObject.AddComponent(mediatorType) as IMediator;
            else
                mediator = mediationBinder.GetMediatorFromPool(mediatorType);

            var injectionResult = context.TryToInjectMediator(mediator, view);
            if (injectionResult)
            {
                var injectedMediatorData = mediationBinder.GetOrCreateInjectedMediatorData(view);
                if (injectedMediatorData == null)
                {
                    Debug.LogError("Injection Data not found! ", view.gameObject);
                    MVCConsole.LogError(ConsoleLogType.Injection, "Injection Data not found! ");
                    return false;
                }
                injectedMediatorData.viewInjector.ViewInjectionCompleted(view);
                injectedMediatorData.mediator = mediator;
            }
            return injectionResult;
        }

        public static void RemoveRegistration(this IView view)
        {
            var viewContext = view.FindViewContext();
            if (viewContext == null)
                return;
            
            var mediationBinder = viewContext.MediationBinder;
            if(mediationBinder == null)
                return;
            
            var injectedMediatorData = mediationBinder.GetOrCreateInjectedMediatorData(view);
            if (injectedMediatorData.mediator == null)
                return;
            
            var viewInjectorComponent = injectedMediatorData.viewInjector;

            var viewInjectorData = viewInjectorComponent.GetViewInjectorData(view);
            if (!viewInjectorData.IsRegistered)
                return;

            var mediator = injectedMediatorData.mediator;
            mediator.OnRemove();

            injectedMediatorData.mediator = null;
            viewInjectorData.IsRegistered = false;

            if (mediator is Object mediatorObject)
                Object.Destroy(mediatorObject as Component);
            else
                mediationBinder.SendMediatorToPool(mediator);
        }
        
        internal static IContext FindViewContext(this IView view)
        {
            var contextRoot = view.FindRoot();
            return contextRoot == null ? null : contextRoot.GetContext();
        }

        internal static IContextRoot FindRoot(this IView view)
        {
            var parent = view.transform.parent;
            if (parent == null)
                return null;

            IContextRoot contextRoot = null;

            while (contextRoot == null)
            {
                contextRoot = parent.GetComponent<IContextRoot>();
                if(parent.parent == null)
                    break;
                parent = parent.parent;
            }
            
            return contextRoot;
        }

        internal static bool IsMediatorMono(this IMediator mediator)
        {
            return mediator is Object;
        }

        internal static ViewBindingData GetBindingData(this IContext mainContext, IView view)
        {
            var viewType = view.GetType();

            var allContexts = mainContext.AllContexts;

            foreach (var context in allContexts)
            {
                var viewBindingData = new ViewBindingData();
                
                var mediationBinder = context.MediationBinder;
                var binding = mediationBinder.GetBinding(viewType);
                if (binding != null)
                {
                    viewBindingData.Binding = binding;
                    viewBindingData.Context = context;
                    return viewBindingData;
                }
            }

            return default;
        }
    }
}