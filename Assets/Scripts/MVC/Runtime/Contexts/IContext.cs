using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public interface IContext
    {
        int InitializeOrder { get; set; }
        
        void Initialize(GameObject contextGameObject, int initializeOrder);
        void Start();
        void Launch();
        
        void MapBindings();
        void PostBindings();
    }
}