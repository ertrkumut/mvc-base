using System.Linq;
using System.Reflection;
using MVC.Runtime.Controller.Binder;
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

        public bool ContextStarted { get; set; }
        
        public MediatorBinder MediatorBinder { get; set; }
        public InjectionBinder InjectionBinder { get; set; }
        public CrossContextInjectionBinder CrossContextInjectionBinder { get; set; }
        public CommandBinder CommandBinder { get; set; }

        public int InitializeOrder { get; set; }

        public void Initialize(GameObject contextGameObject, int initializeOrder, CrossContextInjectionBinder crossContextInjectionBinder)
        {
            _gameObject = contextGameObject;
            InitializeOrder = initializeOrder;
            CrossContextInjectionBinder = crossContextInjectionBinder;
        }

        public void Start()
        {
            ContextStarted = true;
            
            CoreBindings();
            MapBindings();
            PostBindings();
        }

        void IContext.InjectAllInstances()
        {
            var injectedTypes = InjectionBinder.GetInjectedInstances();
            var injectedCrossContextTypes = CrossContextInjectionBinder.GetInjectedInstances();

            injectedTypes = injectedTypes.Concat(injectedCrossContextTypes).ToList();

            foreach (InjectionBinding injectedType in injectedTypes)
            {
                if(injectedType == null)
                    continue;

                this.TryToInjectObject(injectedType.Value);
            }
        }
        
        void IContext.ExecutePostConstructMethods()
        {
            var injectedTypes = InjectionBinder.GetInjectedInstances();
            var injectedCrossContextTypes = CrossContextInjectionBinder.GetInjectedInstances();

            injectedTypes = injectedTypes.Concat(injectedCrossContextTypes).ToList();
            
            foreach (InjectionBinding injectedType in injectedTypes)
            {
                var type = injectedType.Value.GetType();
                var postConstructMethods =
                    type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(PostConstructAttribute), true).Length != 0)
                        .ToList();

                foreach (var postConstructMethod in postConstructMethods)
                {
                    postConstructMethod.Invoke(injectedType.Value, null);
                }
            }
        }

        protected virtual void CoreBindings()
        {
            CommandBinder = new CommandBinder(this);
            InjectionBinder = new InjectionBinder();
            MediatorBinder = InjectionBinder.Bind<MediatorBinder>();
            
            CrossContextInjectionBinder.BindInstance(CrossContextInjectionBinder);
            CrossContextInjectionBinder.BindInstance(CommandBinder, GetType().Name);
            
            InjectionBinder.BindInstance(CommandBinder);
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
            ContextStarted = false;
            MediatorBinder.UnBindAll();
            InjectionBinder.UnBindAll();
        }
    }
}