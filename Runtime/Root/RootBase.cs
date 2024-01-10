using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Root.Utils;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootBase : MonoBehaviour, IRoot
    {
        public List<SubContextData> SubContextTypes;

        protected Dictionary<IContext, SubContextData> _subContexts = new();

        public int initializeOrder;

        internal bool signalsBound;
        internal bool injectionsBound;
        internal bool mediationsBound;
        internal bool commandsBound;
        internal bool hasInitialized;
        internal bool hasSetuped;
        internal bool hasLaunched;
        
        public bool autoBindInjections = true;
        public bool autoBindMediations = true;
        public bool autoInitialize = true;
        public bool autoSetup = true;
        public bool autoLaunch = true;
        
        public IContext Context { get; set; }
        
        protected RootsManager _rootsManager;
        
        public virtual void StartContext(bool forceToStart = false) {}

        public virtual void InitializeSubContexts()
        {
            if(SubContextTypes == null || SubContextTypes.Count == 0)
                return;

            _subContexts = new ();

            var assemblyTypes = AssemblyExtensions.GetAllContextTypes();
            
            foreach (var subContextData in SubContextTypes)
            {
                var contextType = assemblyTypes.FirstOrDefault(x => x.FullName == subContextData.ContextFullName);
                if (contextType == null)
                {
                    MVCConsole.LogError(ConsoleLogType.Context, "Context Type couldn't find! \n" + subContextData.ContextFullName);
                    continue;
                }

                var context = (IContext) Activator.CreateInstance(contextType);
                MVCConsole.Log(ConsoleLogType.Context, "Sub Context Initialized \n" + subContextData.ContextFullName);
                context.Initialize(gameObject, initializeOrder, _rootsManager.injectionBinderCrossContext, new List<IContext>());
                _subContexts.Add(context, subContextData);
            }
        }

        protected virtual void BeforeCreateContext() {}
        protected virtual void AfterCreateBeforeStartContext(){}
        protected virtual void AfterBindingsBeforeInjections(){}
        protected virtual void AfterStarBeforeLaunchContext(){}
        
        public void BindSignals(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoBindInjections && !forceToBind)
                return;
            
            if(signalsBound)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Signals! Context: " + GetType().Name);
            Context.SignalBindings();
            signalsBound = true;

            // foreach (var subContext in _subContexts)
            // {
            //     subContext.Key.SignalBindings();
            //     MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Signals Context: " + subContext.Key.GetType().Name);
            // }
        }

        public void BindInjections(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoBindInjections && !forceToBind)
                return;
            
            if(injectionsBound)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Injections! Context: " + GetType().Name);
            Context.InjectionBindings();
            injectionsBound = true;
        }

        public void BindMediations(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if (!autoBindMediations && !forceToBind)
                return;
            
            if(mediationsBound)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Mediations! Context: " + GetType().Name);
            Context.MediationBindings();
            mediationsBound = true;
        }

        public void BindCommands(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if (!autoBindInjections && !forceToBind)
                return;

            if (commandsBound)
                return;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Commands! Context: " + GetType().Name);
            Context.CommandBindings();
            commandsBound = true;
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

        public void Setup(bool forceToSetup = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoSetup && !forceToSetup)
                return;
            
            if(hasSetuped)
                return;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Setuped! => " + GetType().Name);
            Context.Setup();
            hasSetuped = true;

            foreach (var subContext in _subContexts)
            {
                if (subContext.Value.AutoSetup)
                {
                    MVCConsole.Log(ConsoleLogType.Context, "Sub Context Setuped! => " + subContext.Value.ContextName);
                    subContext.Key.Setup();
                }
            }
        }

        public virtual void Launch(bool forceToLaunch = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoLaunch && !forceToLaunch)
                return;
            
            if(hasLaunched)
                return;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Launched! => " + GetType().Name);
            Context.Launch();
            hasLaunched = true;
        }
    }
}