using System;
using MVC.Runtime.Contexts;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.ViewMediators.Utils
{
    public static class ViewExtensions
    {
        public static void InitializeView(this IMVCView view)
        {
            var viewContext = view.FindViewContext();
            if (viewContext == null)
            {
                Debug.LogError("There is no Context");
                return;
            }

            var mediatorBinder = viewContext.MediatorBinder;
            var binding = mediatorBinder.GetBinding(view.GetType());
            if (binding == null)
            {
                Debug.LogError("There is no view binding! " + view.GetType());
                return;
            }
            
            var mediatorType = binding.Value as Type;
            var mediator = view.gameObject.AddComponent(mediatorType);
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