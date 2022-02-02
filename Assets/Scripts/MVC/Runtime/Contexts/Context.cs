using System.Linq;
using System.Reflection;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Injectable.Binders;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected GameObject _gameObject;

        public MediatorBinder MediatorBinder { get; set; }
        public InjectionBinder InjectionBinder { get; set; }

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

        void IContext.ExecutePostConstructMethods()
        {
            var injectedTypes = InjectionBinder.GetInjectedInstances();
            foreach (object injectedType in injectedTypes)
            {
                var type = injectedType.GetType();
                var postConstructMethods =
                    type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(PostConstructAttribute), true).Length != 0)
                        .ToList();

                foreach (var postConstructMethod in postConstructMethods)
                {
                    postConstructMethod.Invoke(injectedType, null);
                }
            }
        }

        private void CoreBindings()
        {
            MediatorBinder = new MediatorBinder();
            InjectionBinder = new InjectionBinder();
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