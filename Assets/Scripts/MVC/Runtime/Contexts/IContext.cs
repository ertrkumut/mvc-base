using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.CrossContext;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public interface IContext
    {
        int InitializeOrder { get; set; }
        
        MediatorBinder MediatorBinder { get; set; }
        InjectionBinder InjectionBinder { get; set; }
        CrossContextInjectionBinder CrossContextInjectionBinder { get; set; }
        
        void Initialize(GameObject contextGameObject, int initializeOrder, CrossContextInjectionBinder crossContextInjectionBinder);
        void Start();

        internal void InjectAllInstances();
        internal void ExecutePostConstructMethods();
        void Launch();
        
        void MapBindings();
        void PostBindings();
    }
}