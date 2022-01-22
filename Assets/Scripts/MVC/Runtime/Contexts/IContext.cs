using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public interface IContext
    {
        void Initialize(GameObject contextGameObject);
        void Start();
        void Launch();
        
        void MapBindings();
        void PostBindings();
    }
}