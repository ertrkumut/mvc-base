using MVC.Runtime.Contexts;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootBase : MonoBehaviour
    {
        public int initializeOrder;
        
        internal bool signalsBound;
        internal bool injectionsBound;
        internal bool mediationsBound;
        internal bool commandsBound;
        
        public bool bindSignals = true;
        public bool bindInjections = true;
        public bool bindMediations = true;
        public bool bindCommands = true;
        
        public IContext Context { get; set; }
        
        protected RootsManager _rootsManager;
        
        public void BindSignals(bool forceToBind = false)
        {
            if(!bindSignals && !forceToBind)
                return;
            
            if(signalsBound)
                return;
            
            Context.SignalBindings();
            signalsBound = true;
        }

        public void BindInjections(bool forceToBind = false)
        {
            if(!bindInjections && !forceToBind)
                return;
            
            if(injectionsBound)
                return;
            
            Context.InjectionBindings();
            injectionsBound = true;
        }

        public void BindMediations(bool forceToBind = false)
        {
            if (!bindMediations && !forceToBind)
                return;
            
            if(mediationsBound)
                return;
            
            Context.MediationBindings();
            mediationsBound = true;
        }

        public void BindCommands(bool forceToBind = false)
        {
            if (!bindCommands && !forceToBind)
                return;

            if (commandsBound)
                return;
            
            Context.CommandBindings();
            commandsBound = true;
        }
    }
}