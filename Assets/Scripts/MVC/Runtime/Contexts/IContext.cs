using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Binders;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public interface IContext
    {
        int InitializeOrder { get; set; }
        
        MediatorBinder MediatorBinder { get; set; }
        InjectionBinder InjectionBinder { get; set; }
        
        void Initialize(GameObject contextGameObject, int initializeOrder);
        void Start();
        internal void ExecutePostConstructMethods();
        void Launch();
        
        void MapBindings();
        void PostBindings();
    }
}