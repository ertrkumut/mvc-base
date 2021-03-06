using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class ContextRoot<TContextType> : RootBase
        where TContextType : IContext, new()
    {
        protected TContextType _context
        {
            get
            {
                return (TContextType) Context;
            }
            set
            {
                Context = value;
            }
        }

        #region UnityMethods

        private void Awake()
        {
            if(_context != null)
                return;
            
            _rootsManager = RootsManager.Instance;
            CreateContext();
            _rootsManager.RegisterContext(this);
        }

        private void Start()
        {
            _rootsManager.StartContexts();
        }

        private void OnDestroy()
        {
            DestroyContext();
        }

        #endregion

        private void CreateContext()
        {
            BeforeCreateContext();
            
            _context = new TContextType();
            Context = _context;
            _context.Initialize(gameObject, initializeOrder, _rootsManager.injectionBinderCrossContext);
        }
        
        public override void StartContext(bool forceToStart = false)
        {
            if(!autoInitialize && !forceToStart)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Started! Context: " + GetType().Name);
            
            hasInitialized = true;
            AfterCreateBeforeStartContext();

            _context.Start();
            _rootsManager.injectionBinderCrossContext.BindInstance<GameObject>(gameObject, typeof(TContextType).Name);
            
            BindInjections();
            BindSignals();
            BindMediations();
            BindCommands();
            
            _context.InjectAllInstances();
            _context.ExecutePostConstructMethods();
                
            AfterStarBeforeLaunchContext();
            
            _rootsManager.OnContextReady?.Invoke(Context);
        }
        
        public virtual void DestroyContext()
        {
            _rootsManager.UnRegisterContext(this);
            
            _context.DestroyContext();

            signalsBound = false;
            injectionsBound = false;
            mediationsBound = false;
            commandsBound = false;
        }

        protected virtual void BeforeCreateContext(){}

        protected virtual void AfterCreateBeforeStartContext(){}

        protected virtual void AfterStarBeforeLaunchContext(){}
    }
}