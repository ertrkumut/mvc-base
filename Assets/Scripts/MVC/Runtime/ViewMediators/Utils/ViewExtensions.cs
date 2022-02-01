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
        public static bool InitializeView(this IMVCView view)
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

            IMVCMediator mediator;
            
            if(mediatorIsMono)
                mediator = view.gameObject.AddComponent(mediatorType) as IMVCMediator;
            else
                mediator = (IMVCMediator) Activator.CreateInstance(mediatorType);

            return viewContext.TryToInjectMediator(mediator, view);
        }

        public static IContext FindViewContext(this IMVCView view)
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