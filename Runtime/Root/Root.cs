using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using UnityEngine;

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
            
            _context = new TContextType();
            Context = _context;

            if (_subContexts == null)
                _subContexts = new Dictionary<IContext, SubContextData>();
            _context.Initialize(gameObject, initializeOrder, _rootsManager.injectionBinderCrossContext, _subContexts.Keys.ToList());
        }
        
        public override void StartContext(bool forceToStart = false)
        {
            if(!autoInitialize && !forceToStart)
                return;

            hasInitialized = true;
            AfterCreateBeforeStartContext();

            MVCConsole.Log(ConsoleLogType.Context, "Context Started! Context: " + GetType().Name);
            _context.Start();
            
            foreach (var subContextData in _subContexts)
            {
                subContextData.Key.Start();
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Started! Context: " + subContextData.Key.GetType().Name);
            }

            BindInjections();
            BindSignals();
            BindMediations();
            BindCommands();
            
            _context.InjectAllInstances();
            _context.ExecutePostConstructMethods();
            MVCConsole.Log(ConsoleLogType.Context, "Context Executed Post Construct Methods! => " + _context.GetType().Name);
            
            foreach (var subContextData in _subContexts)
            {
                subContextData.Key.InjectAllInstances(true);
                //MVCConsole.Log(ConsoleLogType.Context, "Sub Context Started! Context: " + subContextData.Key.GetType().Name);
            }
            foreach (var subContextData in _subContexts)
            {
                subContextData.Key.ExecutePostConstructMethods();
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Executed Post Construct Methods! => " + subContextData.Key.GetType().Name);
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