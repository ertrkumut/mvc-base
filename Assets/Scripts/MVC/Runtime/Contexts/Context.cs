using MVC.Runtime.Injectable.Binders;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected GameObject _gameObject;

        protected MediatorBinder _mediatorBinder;
        
        public int InitializeOrder { get; set; }

        public void Initialize(GameObject contextGameObject, int initializeOrder)
        {
            _gameObject = contextGameObject;
            InitializeOrder = initializeOrder;
        }

        public void Start()
        {
            CoreBindings();
            MapBindings();
            PostBindings();
        }

        private void CoreBindings()
        {
            _mediatorBinder = new MediatorBinder();
        }

        public virtual void MapBindings()
        {
            
        }

        public virtual void PostBindings()
        {
            
        }
        
        public void Launch()
        {
        }
    }
}