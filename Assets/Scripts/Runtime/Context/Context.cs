using UnityEngine;

namespace Runtime.Context
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