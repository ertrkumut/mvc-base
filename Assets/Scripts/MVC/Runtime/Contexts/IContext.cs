using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.CrossContext;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public interface IContext
    {
        int InitializeOrder { get; set; }
        bool ContextStarted { get; set; }
        MediatorBinder MediatorBinder { get; set; }
        InjectionBinder InjectionBinder { get; set; }
        CrossContextInjectionBinder CrossContextInjectionBinder { get; set; }
        ICommandBinder CommandBinder { get; set; }
        void Initialize(GameObject contextGameObject, int initializeOrder, CrossContextInjectionBinder crossContextInjectionBinder);
        void Start();

        internal void InjectAllInstances();
        internal void ExecutePostConstructMethods();

        void MapBindings();
        void PostBindings();
        
        void Launch();
        void DestroyContext();
    }
}