using System;
using System.Collections.Generic;
using MVC.Editor.Console;
using MVC.Runtime.Attributes;
using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Bind.Bindings.Mediator;
using MVC.Runtime.Console;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Injectable.Mediator;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Mediator;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Binders
{
    [HideInModelViewer]
    public class MediationBinder : Binder<MediatorBinding>
    {
        private Dictionary<IView, InjectedMediatorData> _injectedMediators;
        private MediatorCreatorController _mediatorCreatorController;

        public MediationBinder()
        {
            _injectedMediators = new Dictionary<IView, InjectedMediatorData>();
            _mediatorCreatorController = RootsManager.Instance.mediatorCreatorController;
        }
        
        public new virtual MediatorBinding Bind<TKeyType>()
            where TKeyType : IView
        {
            return base.Bind<TKeyType>();
        }
        
        public override MediatorBinding Bind(object key)
        {
            var viewType = key.GetType();
            if (!typeof(IView).IsAssignableFrom(viewType))
            {
                Debug.LogError("Binding View require to inherit from IMVCView interface! " + viewType.Name);
                MVCConsole.LogError(ConsoleLogType.Injection, "Binding View require to inherit from IMVCView interface! " + viewType.Name);
                return null;
            }
            
            return base.Bind(key);
        }

        internal IMediator GetMediatorFromPool(Type mediatorType)
        {
            return _mediatorCreatorController.GetMediator(mediatorType);
        }
        
        internal void SendMediatorToPool(IMediator mediator)
        {
            _mediatorCreatorController.ReturnMediatorToPool(mediator);
        }

        internal InjectedMediatorData GetOrCreateInjectedMediatorData(IView view)
        {
            InjectedMediatorData mediatorData = null;
            if (_injectedMediators.ContainsKey(view))
                mediatorData = _injectedMediators[view];
            else
            {
                mediatorData = new InjectedMediatorData
                {
                    view = view,
                    viewInjector = view.transform.GetComponent<ViewInjector>()
                };
                _injectedMediators.Add(view, mediatorData);
            }
            
            return mediatorData;
        }
        internal InjectedMediatorData GetInjectedMediatorData(IView view)
        {
            InjectedMediatorData mediatorData = null;
            if (_injectedMediators.ContainsKey(view))
                mediatorData = _injectedMediators[view];
            
            return mediatorData;
        }
    }
}