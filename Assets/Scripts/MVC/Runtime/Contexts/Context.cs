using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected GameObject _gameObject;
        
        public void Initialize(GameObject contextGameObject)
        {
            _gameObject = contextGameObject;
        }

        public void Start()
        {
            MapBindings();
            PostBindings();
        }

        public void Launch()
        {
        }

        public virtual void MapBindings()
        {
            
        }

        public virtual void PostBindings()
        {
            
        }
    }
}