using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Bind.Bindings.Pool;
using MVC.Runtime.Contexts;
using MVC.Runtime.Injectable.CrossContext;
using MVC.Runtime.ViewMediators.Mediator;

namespace MVC.Runtime.Root
{
    public class RootsManager
    {
        private static RootsManager _instance;
        public static RootsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RootsManager();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        public InjectionBinderCrossContext injectionBinderCrossContext;
        public MediatorCreatorController mediatorCreatorController;
        public BindingPoolController bindingPoolController;

        public Action<IContext> OnContextReady;

        private bool _contextsStarted;
        
        private List<IContextRoot> _contextRootList;
        
        public void Initialize()
        {
            _contextRootList = new List<IContextRoot>();
            
            bindingPoolController = new BindingPoolController();
            injectionBinderCrossContext = new InjectionBinderCrossContext();
            mediatorCreatorController = new MediatorCreatorController();
        }

        public void RegisterContext(IContextRoot contextRoot)
        {
            _contextRootList.Add(contextRoot);
        }

        public void UnRegisterContext(IContextRoot contextRoot)
        {
            _contextsStarted = false;
            _contextRootList.Remove(contextRoot);
        }

        public void StartContexts()
        {
            if(_contextsStarted)
                return;

            _contextsStarted = true;

            var unreadyContextList = _contextRootList.
                Where(context => !context.GetContext().ContextStarted)
                .OrderBy(x => x.GetContext().InitializeOrder)
                .ToList();
            
            foreach (var contextRoot in unreadyContextList)
            {
                contextRoot.StartContext();
                OnContextReady?.Invoke(contextRoot.GetContext());
            }

            foreach (var contextRoot in _contextRootList)
            {
                contextRoot.GetContext().Launch();
            }
        }

        public bool IsContextReady(IContext context)
        {
            return _contextRootList
                .FirstOrDefault(contextRoot => contextRoot.GetContext() == context).GetContext()
                .ContextStarted;
        }
    }
}