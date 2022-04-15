using System.Linq;
using System.Reflection;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Function.Provider;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.CrossContext;
using MVC.Runtime.Injectable.Utils;
using MVC.Runtime.Provider.Update;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected GameObject _gameObject;

        public bool ContextStarted { get; set; }
        
        public MediationBinder MediationBinder { get; set; }
        public InjectionBinder InjectionBinder { get; set; }
        public InjectionBinderCrossContext InjectionBinderCrossContext { get; set; }
        public ICommandBinder CommandBinder { get; set; }

        public int InitializeOrder { get; set; }

        public void Initialize(GameObject contextGameObject, int initializeOrder, InjectionBinderCrossContext injectionBinderCrossContext)
        {
            _gameObject = contextGameObject;
            InitializeOrder = initializeOrder;
            InjectionBinderCrossContext = injectionBinderCrossContext;
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
            var injectedCrossContextTypes = InjectionBinderCrossContext.GetInjectedInstances();

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
            var injectedCrossContextTypes = InjectionBinderCrossContext.GetInjectedInstances();

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
            InjectionBinder = new InjectionBinder();
            MediationBinder = InjectionBinder.Bind<MediationBinder>();
            
            InjectionBinderCrossContext.BindInstance(InjectionBinderCrossContext);

            CommandBinder = InjectionBinder.Bind<ICommandBinder, CommandBinder>();
            ((CommandBinder) CommandBinder).Context = this;

            var functionProvider = (FunctionProvider) InjectionBinder.Bind<IFunctionProvider, FunctionProvider>();
            functionProvider.Context = this;
            
            InjectionBinderCrossContext.BindMonoBehaviorInstance<IUpdateProvider, UpdateProvider>();
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
            MediationBinder.UnBindAll();
            InjectionBinder.UnBindAll();
        }
    }
}