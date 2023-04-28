using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;

namespace MVC.Runtime.Root
{
    public class Root<TContextType> : RootBase
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
            MVCConsole.Log(ConsoleLogType.Context, "Context - Initialize "+ GetType().Name);
            _context = new TContextType();
            Context = _context;

            _context.Initialize(gameObject, initializeOrder, _rootsManager.injectionBinderCrossContext, _subContexts.Keys.ToList());
            
            
        }
        
        public override void StartContext(bool forceToStart = false)
        {
            if (!autoInitialize && !forceToStart)
                return;

            hasInitialized = true;
            AfterCreateBeforeStartContext();

            MVCConsole.Log(ConsoleLogType.Context, "Context Started! Context: " + GetType().Name);
            _context.Start();
            BindInjections();
            BindSignals();
            BindMediations();
            BindCommands();
            
            foreach (var subContext in _subContexts)
            {
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Started! Context: " + subContext.Key.GetType().Name);
                subContext.Key.Start();
                
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Injections Context: " + subContext.Key.GetType().Name);
                subContext.Key.InjectionBindings();
                
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Signals Context: " + subContext.Key.GetType().Name);
                subContext.Key.SignalBindings();
                
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Mediations Context: " + subContext.Key.GetType().Name);
                subContext.Key.MediationBindings();
                
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Commands Context: " + subContext.Key.GetType().Name);
                subContext.Key.CommandBindings();
            }

            MVCConsole.Log(ConsoleLogType.Context, "Context InjectAllInstances => " + _context.GetType().Name);
            _context.InjectAllInstances();
            MVCConsole.Log(ConsoleLogType.Context, "Context Executed Post Construct Methods! => " + _context.GetType().Name);
            _context.ExecutePostConstructMethods();
            
            foreach (var subContextData in _subContexts)
            {
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context InjectAllInstances! => " + subContextData.Key.GetType().Name);
                subContextData.Key.InjectAllInstances(true);
            }
            foreach (var subContextData in _subContexts)
            {
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Executed Post Construct Methods! => " + subContextData.Key.GetType().Name);
                subContextData.Key.ExecutePostConstructMethods();
            }
            
            AfterStarBeforeLaunchContext();
            
            _rootsManager.OnContextReady?.Invoke(Context);
        }
        
        public virtual void DestroyContext()
        {
            _rootsManager.UnRegisterContext(this);
            
            _context.DestroyContext();
            
            injectionsBound = false;
            mediationsBound = false;
            commandsBound = false;
        }
    }
}