using System.Collections.Generic;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.CrossContext;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public interface IContext
    {
        List<IContext> SubContexts { get; set; }
        List<IContext> AllContexts { get; set; }

        int InitializeOrder { get; set; }
        bool ContextStarted { get; set; }
        MediationBinder MediationBinder { get; set; }
        InjectionBinder InjectionBinder { get; set; }
        InjectionBinderCrossContext InjectionBinderCrossContext { get; set; }
        ICommandBinder CommandBinder { get; set; }
        void Initialize(GameObject contextGameObject, int initializeOrder, InjectionBinderCrossContext injectionBinderCrossContext, List<IContext> subContexts);
        void Start();

        internal void InjectAllInstances(bool isSubContext = false);
        internal void ExecutePostConstructMethods();
        
        void SignalBindings();
        void InjectionBindings();
        void MediationBindings();
        void CommandBindings();
        void PostBindings();
        void Setup();
        void Launch();
        void DestroyContext();
    }
}