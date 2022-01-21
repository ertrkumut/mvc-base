using UnityEngine;

namespace Runtime.Context
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