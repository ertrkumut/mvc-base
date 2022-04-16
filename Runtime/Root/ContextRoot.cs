using MVC.Runtime.Contexts;

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
        
        public override void StartContext()
        {
            AfterCreateBeforeStartContext();

            _context.Start();
            
            BindSignals();
            BindInjections();
            BindMediations();
            BindCommands();
            
            _context.InjectAllInstances();
            _context.ExecutePostConstructMethods();
                
            AfterStarBeforeLaunchContext();
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

        private void BeforeCreateContext(){}

        private void AfterCreateBeforeStartContext(){}

        private void AfterStarBeforeLaunchContext(){}
    }
}