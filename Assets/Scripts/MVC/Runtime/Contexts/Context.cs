using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected GameObject _gameObject;

        public int InitializeOrder { get; set; }

        public void Initialize(GameObject contextGameObject, int initializeOrder)
        {
            _gameObject = contextGameObject;
            InitializeOrder = initializeOrder;
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