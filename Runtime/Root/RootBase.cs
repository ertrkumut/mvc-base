using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Attributes;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Root.Utils;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootBase : MonoBehaviour, IRoot
    {
        protected Dictionary<IContext, SubContextData> _subContexts = new();

        internal bool SignalsBound;
        internal bool InjectionsBound;
        internal bool MediationsBound;
        internal bool CommandsBound;
        internal bool HasInitialized;
        internal bool HasSetupCompleted;
        internal bool HasLaunched;
        
        [HideInInspector] public List<SubContextData> SubContextTypes;
        [HideInInspector] public int InitializeOrder;
        
        [HideInInspector] public bool AutoBindInjections = true;
        [HideInInspector] public bool AutoBindMediations = true;
        [HideInInspector] public bool AutoInitialize = true;
        [HideInInspector] public bool AutoLaunch = true;
        
        public IContext Context { get; set; }
        
        protected RootsManager _rootsManager;
        
        public virtual void StartContext(bool forceToStart = false) {}

        public virtual void InitializeSubContexts()
        {
            if(SubContextTypes == null || SubContextTypes.Count == 0)
                return;

            _subContexts = new ();

            var subContextAttributes = GetType().GetCustomAttributes(typeof(SubContextAttribute),true);

            foreach (var attribute in subContextAttributes)
            {
                if(attribute is not SubContextAttribute subContextAttribute)
                    continue;
                
                CreateSubContextAndInitialize(subContextAttribute.ContextType, new SubContextData
                {
                    ContextFullName = subContextAttribute.ContextType.FullName,
                    ContextName = subContextAttribute.ContextType.Name,
                    AutoSetup = subContextAttribute.AutoSetup
                });
            }
            
            var assemblyTypes = AssemblyExtensions.GetAllContextTypes();
            
            foreach (var subContextData in SubContextTypes)
            {
                var contextType = assemblyTypes.FirstOrDefault(x => x.FullName == subContextData.ContextFullName);
                if (contextType == null)
                {
                    MVCConsole.LogError(ConsoleLogType.Context, "Context Type couldn't find! \n" + subContextData.ContextFullName);
                    continue;
                }

                CreateSubContextAndInitialize(contextType, subContextData);
            }
        }

        protected void CreateSubContextAndInitialize(Type contextType, SubContextData subContextData)
        {
            foreach (var keyPairOfSubContext in _subContexts)
            {
                if(keyPairOfSubContext.Key.GetType() == contextType)
                    return;
            }
            
            var context = (IContext) Activator.CreateInstance(contextType);
            MVCConsole.Log(ConsoleLogType.Context, "Sub Context Initialized \n" + subContextData.ContextFullName);
            context.Initialize(gameObject, InitializeOrder, _rootsManager.injectionBinderCrossContext, new List<IContext>());
            _subContexts.Add(context, subContextData);
        }
        
        protected virtual void BeforeCreateContext()
        {
            InitializeSubContexts();
        }

        protected virtual void AfterCreateBeforeStartContext(){}
        protected virtual void AfterBindingsBeforeInjections(){}
        protected virtual void AfterStarBeforeLaunchContext(){}
        
        public void BindSignals(bool forceToBind = false)
        {
            if(!HasInitialized)
                return;
            
            if(!AutoBindInjections && !forceToBind)
                return;
            
            if(SignalsBound)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Signals! Context: " + GetType().Name);
            Context.SignalBindings();
            SignalsBound = true;

            // foreach (var subContext in _subContexts)
            // {
            //     subContext.Key.SignalBindings();
            //     MVCConsole.Log(ConsoleLogType.Context, "Sub Context Bind Signals Context: " + subContext.Key.GetType().Name);
            // }
        }

        public void BindInjections(bool forceToBind = false)
        {
            if(!HasInitialized)
                return;
            
            if(!AutoBindInjections && !forceToBind)
                return;
            
            if(InjectionsBound)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Injections! Context: " + GetType().Name);
            Context.InjectionBindings();
            InjectionsBound = true;
        }

        public void BindMediations(bool forceToBind = false)
        {
            if(!HasInitialized)
                return;
            
            if (!AutoBindMediations && !forceToBind)
                return;
            
            if(MediationsBound)
                return;

            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Mediations! Context: " + GetType().Name);
            Context.MediationBindings();
            MediationsBound = true;
        }

        public void BindCommands(bool forceToBind = false)
        {
            if(!HasInitialized)
                return;
            
            if (!AutoBindInjections && !forceToBind)
                return;

            if (CommandsBound)
                return;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Commands! Context: " + GetType().Name);
            Context.CommandBindings();
            CommandsBound = true;
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

        public void Setup()
        {
            if(!HasInitialized)
                return;
            
            if(HasSetupCompleted)
                return;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Setuped! => " + GetType().Name);
            Context.Setup();
            HasSetupCompleted = true;

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
            if(!HasInitialized)
                return;
            
            if(!AutoLaunch && !forceToLaunch)
                return;
            
            if(HasLaunched)
                return;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Launched! => " + GetType().Name);
            Context.Launch();
            HasLaunched = true;
        }
    }
}