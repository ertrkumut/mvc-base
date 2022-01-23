using System;
using System.Collections.Generic;
using System.Linq;

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

        public Action OnContextsReady;
        
        public bool ContextsReady { get; private set; }
        
        private bool _contextsStarted;
        
        private List<IContextRoot> _contextRootList;
        
        public void Initialize()
        {
            _contextRootList = new List<IContextRoot>();
        }

        public void RegisterContext(IContextRoot contextRoot)
        {
            _contextRootList.Add(contextRoot);
        }

        public void StartContexts()
        {
            if(_contextsStarted)
                return;

            _contextsStarted = true;

            _contextRootList = _contextRootList.OrderBy(x => x.GetContext().InitializeOrder).ToList();
            foreach (var contextRoot in _contextRootList)
            {
                contextRoot.StartContext();
            }

            ContextsReady = true;
            OnContextsReady?.Invoke();
            
            foreach (var contextRoot in _contextRootList)
            {
                contextRoot.GetContext().Launch();
            }
            
        }
    }
}