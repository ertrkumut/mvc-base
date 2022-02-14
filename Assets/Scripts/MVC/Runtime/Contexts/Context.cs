using System.Linq;
using System.Reflection;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.CrossContext;
using MVC.Runtime.Injectable.Utils;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected GameObject _gameObject;

        public MediatorBinder MediatorBinder { get; set; }
        public InjectionBinder InjectionBinder { get; set; }
        public CrossContextInjectionBinder CrossContextInjectionBinder { get; set; }

        public int InitializeOrder { get; set; }

        public void Initialize(GameObject contextGameObject, int initializeOrder, CrossContextInjectionBinder crossContextInjectionBinder)
        {
            _gameObject = contextGameObject;
            InitializeOrder = initializeOrder;
            CrossContextInjectionBinder = crossContextInjectionBinder;
        }

        public void Start()
        {
            CoreBindings();
            MapBindings();
            PostBindings();
        }

        void IContext.InjectAllInstances()
        {
            var injectedTypes = InjectionBinder.GetInjectedInstances();
            var injectedCrossContextTypes = CrossContextInjectionBinder.GetInjectedInstances();

            injectedTypes = injectedTypes.Concat(injectedCrossContextTypes).ToList();

            foreach (object injectedType in injectedTypes)
            {
                if(injectedType == null)
                    continue;

                this.TryToInjectObject(injectedType);
            }
        }
        
        void IContext.ExecutePostConstructMethods()
        {
            var injectedTypes = InjectionBinder.GetInjectedInstances();
            var injectedCrossContextTypes = CrossContextInjectionBinder.GetInjectedInstances();

            injectedTypes = injectedTypes.Concat(injectedCrossContextTypes).ToList();
            
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

        protected virtual void CoreBindings()
        {
            InjectionBinder = new InjectionBinder();
            MediatorBinder = InjectionBinder.Bind<MediatorBinder>();
            
            CrossContextInjectionBinder.BindInstance(CrossContextInjectionBinder);
        }

        public virtual void MapBindings()
        {
            
        }

        public virtual void PostBindings()
        {
            
        }
        
        public virtual void Launch()
        {
        }

        public virtual void DestroyContext()
        {

        }
    }
}