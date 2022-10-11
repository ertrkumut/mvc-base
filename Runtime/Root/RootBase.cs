using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootBase : MonoBehaviour, IContextRoot
    {
        public List<SubContextData> SubContextTypes;

        protected Dictionary<IContext, SubContextData> _subContexts;

        public int initializeOrder;

        internal bool signalsBound;
        internal bool injectionsBound;
        internal bool mediationsBound;
        internal bool commandsBound;
        internal bool hasInitialized;
        internal bool hasLaunched;
        
        public bool autoBindInjections = true;
        public bool autoBindMediations = true;
        public bool autoInitialize = true;
        public bool autoLaunch = true;
        
        public IContext Context { get; set; }
        
        protected RootsManager _rootsManager;
        
        public virtual void StartContext(bool forceToStart = false) {}

        public virtual void InitializeSubContexts()
        {
            if(SubContextTypes == null || SubContextTypes.Count == 0)
                return;

            _subContexts = new Dictionary<IContext, SubContextData>();
            
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("Assembly-CSharp,"));
            var assemblyTypes = assembly.GetTypes();
            
            foreach (var subContextData in SubContextTypes)
            {
                var contextType = assemblyTypes.FirstOrDefault(x => x.FullName == subContextData.ContextFullName);
                if (contextType == null)
                {
                    MVCConsole.LogError(ConsoleLogType.Context, "Context Type couldn't find! \n" + subContextData.ContextFullName);
                    continue;
                }

                var context = (IContext) Activator.CreateInstance(contextType);
                context.Initialize(gameObject, initializeOrder, _rootsManager.injectionBinderCrossContext, new List<IContext>());
                _subContexts.Add(context, subContextData);
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Initialized \n" + subContextData.ContextFullName);
            }
        }

        protected virtual void BeforeCreateContext()
        {
            InitializeSubContexts();
        }
        protected virtual void AfterCreateBeforeStartContext(){}
        protected virtual void AfterStarBeforeLaunchContext(){}
        
        public void BindSignals(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoBindInjections && !forceToBind)
                return;
            
            if(signalsBound)
                return;
            signalsBound = true;
            
            foreach (var subContext in _subContexts)
            {
                subContext.Key.SignalBindings();
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Signals Context: " + subContext.Key.GetType().Name);
            }
            
            Context.SignalBindings();

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Signals! Context: " + GetType().Name);
        }

        public void BindInjections(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoBindInjections && !forceToBind)
                return;
            
            if(injectionsBound)
                return;
            
            foreach (var subContext in _subContexts)
            {
                subContext.Key.InjectionBindings();
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Injections Context: " + subContext.Key.GetType().Name);
            }
            
            Context.InjectionBindings();
            injectionsBound = true;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Injections! Context: " + GetType().Name);
        }

        public void BindMediations(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if (!autoBindMediations && !forceToBind)
                return;
            
            if(mediationsBound)
                return;
            
            foreach (var subContext in _subContexts)
            {
                subContext.Key.MediationBindings();
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Mediations Context: " + subContext.Key.GetType().Name);
            }
            
            Context.MediationBindings();
            mediationsBound = true;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Mediations! Context: " + GetType().Name);
        }

        public void BindCommands(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if (!autoBindInjections && !forceToBind)
                return;

            if (commandsBound)
                return;
            
            foreach (var subContext in _subContexts)
            {
                subContext.Key.CommandBindings();
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Commands Context: " + subContext.Key.GetType().Name);
            }
            
            Context.CommandBindings();
            commandsBound = true;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Commands! Context: " + GetType().Name);
        }
        
        public IContext GetContext()
        {
            return Context;
        }

        public List<IContext> GetSubContexts()
        {
            return _subContexts.Keys.ToList();
        }

        public List<IContext> GetAllContexts()
        {
            var list = _subContexts.Keys.ToList();
            list.Add(GetContext());
            return list;
        }

        public virtual void Launch(bool forceToLaunch = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoLaunch && !forceToLaunch)
                return;
            
            if(hasLaunched)
                return;
            
            foreach (var subContext in _subContexts)
            {
                if(subContext.Value.autoLaunch)
                {
                    subContext.Key.Launch();
                    MVCConsole.Log(ConsoleLogType.Context, "Sub Context Launched \n" + subContext.Value.ContextFullName);
                }
            }
            
            Context.Launch();
            hasLaunched = true;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Launched! Context: " + GetType().Name);
        }
    }
}