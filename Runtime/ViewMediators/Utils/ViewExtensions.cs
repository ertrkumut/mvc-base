﻿using System;
using MVC.Editor.Console;
using MVC.Runtime.Bind.Bindings;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Injectable.Mediator;
using MVC.Runtime.Injectable.Utils;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Mediator;
using MVC.Runtime.ViewMediators.View;
using MVC.Runtime.ViewMediators.View.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.Utils
{
    public static class ViewExtensions
    {
        public static bool Register(this IView view)
        {
            if (view.IsRegistered)
            {
                Debug.LogWarning("View is already registered. \nviewType: " + view.GetType().Name);
                MVCConsole.LogWarning(ConsoleLogType.Injection, "View is already registered. \nviewType: " + view.GetType().Name);
                return false;
            }
            
            var context = view.FindViewContext();
            if (context == null)
            {
                Debug.LogError("There is no Context \nviewType: " + view.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Injection, "There is no Context \nviewType: " + view.GetType().Name);
                return false;
            }

            return view.Register(context);
        }
        
        internal static bool Register(this IView view, ViewInjectorData injectorData)
        {
            if (view.IsRegistered)
            {
                Debug.LogWarning("View is already registered. \nviewType: " + view.GetType().Name);
                MVCConsole.LogWarning(ConsoleLogType.Injection, "View is already registered. \nviewType: " + view.GetType().Name);
                return false;
            }
            
            if (injectorData.SelectedRoot == null)
            {
                return view.Register();
            }

            var context = injectorData.SelectedRoot.GetContext();
            if (context == null)
            {
                Debug.LogError("There is no Context \nviewType: " + view.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Injection, "There is no Context \nviewType: " + view.GetType().Name);
                return false;
            }

            return view.Register(context);
        }

        private static bool Register(this IView view, IContext context)
        {
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

            var viewInjector = view.transform.GetComponent<ViewInjector>();
            var viewInjectionData = viewInjector.GetViewInjectorData(view);

            if (viewInjectionData.InjectableView)
                InjectionExtensions.TryToInjectObject(context, view);
            
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

        public static bool FindMediationBinder(this IView view, out MediationBinder mediationBinder)
        {
            var viewContext = view.FindViewContext();
            mediationBinder = null;
            
            if (viewContext == null)
                return false;
            
            mediationBinder = viewContext.MediationBinder;
            if(mediationBinder == null)
                return false;
            
            var injectedMediatorData = mediationBinder.GetInjectedMediatorData(view);
            if (injectedMediatorData?.mediator != null)
                return true;

            var viewSubContexts = view.FindViewContext().SubContexts;
            foreach (var viewSubContext in viewSubContexts)
            {
                mediationBinder = viewSubContext.MediationBinder;
                if(mediationBinder == null)
                    return false;
                
                injectedMediatorData = mediationBinder.GetInjectedMediatorData(view);
                if (injectedMediatorData?.mediator != null)
                    return true;

            }
            return false;
        }

        public static void UnRegister(this IView view)
        {
            if (!view.FindMediationBinder(out var mediationBinder))
                return;
            
            var injectedMediatorData = mediationBinder.GetInjectedMediatorData(view);
            
            var viewInjectorComponent = injectedMediatorData.viewInjector;

            var viewInjectorData = viewInjectorComponent.GetViewInjectorData(view);
            if (!viewInjectorData.IsRegistered)
                return;

            var mediator = injectedMediatorData.mediator;
            mediator.OnRemove();

            view.IsRegistered = false;
            
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

        internal static IRoot FindRoot(this IView view)
        {
            var parent = view.transform.parent;
            if (parent == null)
                return null;

            IRoot root = null;

            while (root == null)
            {
                root = parent.GetComponent<IRoot>();
                if(parent.parent == null)
                    break;
                parent = parent.parent;
            }
            
            return root;
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