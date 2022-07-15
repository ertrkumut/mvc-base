using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootBase : MonoBehaviour, IContextRoot
    {
        public int initializeOrder;
        
        internal bool signalsBound;
        internal bool injectionsBound;
        internal bool mediationsBound;
        internal bool commandsBound;
        internal bool hasInitialized;
        internal bool hasLaunched;
        
        public bool autoBindSignals = true;
        public bool autoBindInjections = true;
        public bool autoBindMediations = true;
        public bool autoBindCommands = true;

        public bool autoInitialize = true;
        public bool autoLaunch = true;
        
        public IContext Context { get; set; }
        
        protected RootsManager _rootsManager;
        
        public virtual void StartContext(bool forceToStart = false) {}

        public void BindSignals(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoBindSignals && !forceToBind)
                return;
            
            if(signalsBound)
                return;
            
            Context.SignalBindings();
            signalsBound = true;
            
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
            
            Context.MediationBindings();
            mediationsBound = true;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Mediations! Context: " + GetType().Name);
        }

        public void BindCommands(bool forceToBind = false)
        {
            if(!hasInitialized)
                return;
            
            if (!autoBindCommands && !forceToBind)
                return;

            if (commandsBound)
                return;
            
            Context.CommandBindings();
            commandsBound = true;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Bind Commands! Context: " + GetType().Name);
        }
        
        public IContext GetContext()
        {
            return Context;
        }
        
        public virtual void Launch(bool forceToLaunch = false)
        {
            if(!hasInitialized)
                return;
            
            if(!autoLaunch && !forceToLaunch)
                return;
            
            if(hasLaunched)
                return;
            
            Context.Launch();
            hasLaunched = true;
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Launched! Context: " + GetType().Name);
        }
    }
}