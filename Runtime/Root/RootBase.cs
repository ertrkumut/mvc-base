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
        internal bool hasLaunched;
        
        public bool autoBindSignals = true;
        public bool autoBindInjections = true;
        public bool autoBindMediations = true;
        public bool autoBindCommands = true;

        public bool autoLaunch = true;
        
        public IContext Context { get; set; }
        
        protected RootsManager _rootsManager;
        
        public virtual void StartContext() {}

        public void BindSignals(bool forceToBind = false)
        {
            if(!autoBindSignals && !forceToBind)
                return;
            
            if(signalsBound)
                return;
            
            Context.SignalBindings();
            signalsBound = true;
        }

        public void BindInjections(bool forceToBind = false)
        {
            if(!autoBindInjections && !forceToBind)
                return;
            
            if(injectionsBound)
                return;
            
            Context.InjectionBindings();
            injectionsBound = true;
        }

        public void BindMediations(bool forceToBind = false)
        {
            if (!autoBindMediations && !forceToBind)
                return;
            
            if(mediationsBound)
                return;
            
            Context.MediationBindings();
            mediationsBound = true;
        }

        public void BindCommands(bool forceToBind = false)
        {
            if (!autoBindCommands && !forceToBind)
                return;

            if (commandsBound)
                return;
            
            Context.CommandBindings();
            commandsBound = true;
        }
        
        public IContext GetContext()
        {
            return Context;
        }
        
        public virtual void Launch(bool forceToLaunch = false)
        {
            if(!autoLaunch && !forceToLaunch)
                return;
            
            if(hasLaunched)
                return;
            
            Context.Launch();
            hasLaunched = true;
        }
    }
}