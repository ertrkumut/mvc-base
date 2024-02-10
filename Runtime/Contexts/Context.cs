using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Function.Provider;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.Binders;
using MVC.Runtime.Injectable.CrossContext;
using MVC.Runtime.Injectable.Utils;
using MVC.Runtime.Provider.Coroutine;
using MVC.Runtime.Provider.Update;
using UnityEngine;

namespace MVC.Runtime.Contexts
{
    public class Context : IContext
    {
        protected virtual string LocalInjectionBinderName => GetType().Name;
        
        protected GameObject _gameObject;
        public bool ContextStarted { get; set; }
        public MediationBinder MediationBinder { get; set; }
        public InjectionBinder InjectionBinder { get; set; }
        public InjectionBinderCrossContext InjectionBinderCrossContext { get; set; }
        public ICommandBinder CommandBinder { get; set; }
        public List<IContext> SubContexts { get; set; }
        public List<IContext> AllContexts { get; set; }
        
        public int InitializeOrder { get; set; }

        public void Initialize(GameObject contextGameObject, int initializeOrder, InjectionBinderCrossContext injectionBinderCrossContext, List<IContext> subContexts)
        {
            _gameObject = contextGameObject;
            InitializeOrder = initializeOrder;
            InjectionBinderCrossContext = injectionBinderCrossContext;
            SubContexts = subContexts;

            AllContexts = subContexts;
            AllContexts.Insert(0, this);
        }

        public void Start()
        {
            ContextStarted = true;
            
            CoreBindings();
        }

        void IContext.InjectAllInstances(bool isSubContext = false)
        {
            var injectionBindings = InjectionBinder.GetAllInjectionBindings();
            var crossContextInjectedBindings = InjectionBinderCrossContext.GetAllInjectionBindings();

            injectionBindings = injectionBindings.Concat(crossContextInjectedBindings).ToList();


            foreach (InjectionBinding binding in injectionBindings)
            {
                if(binding == null)
                    continue;

                if (binding.BindedContext == null)
                    this.TryToInjectObject(binding.Value);
                else
                    binding.TryToInjectObject();
            }
        }
        
        void IContext.ExecutePostConstructMethods()
        {
            var injectedTypes = InjectionBinder.GetAllInjectionBindings();
            var injectedCrossContextTypes = InjectionBinderCrossContext.GetAllInjectionBindings();

            injectedTypes = injectedTypes.Concat(injectedCrossContextTypes).ToList();
            
            foreach (InjectionBinding injectedType in injectedTypes)
            {
                if(InjectionBinderCrossContext.PostConstructedObjects.Contains(injectedType.Value))
                    continue;
                
                PostConstructUtils.ExecutePostConstructMethod(injectedType.Value);
                
                InjectionBinderCrossContext.PostConstructedObjects.Add(injectedType.Value);
            }
        }

        protected virtual void CoreBindings()
        {
            InjectionBinder = new InjectionBinder();
            //InjectionBinder.SetBindedContext(this);
            
            MediationBinder = InjectionBinder.Bind<MediationBinder>();
            InjectionBinder.BindInstance(InjectionBinder, LocalInjectionBinderName);
            InjectionBinder.SetBindedContext(this);
            
            InjectionBinderCrossContext.BindInstance(InjectionBinderCrossContext);
            InjectionBinderCrossContext.SetBindedContext(this);

            CommandBinder = InjectionBinder.Bind<ICommandBinder, CommandBinder>();
            ((CommandBinder) CommandBinder).Context = this;

            var functionProvider = (FunctionProvider) InjectionBinder.Bind<IFunctionProvider, FunctionProvider>();
            functionProvider.Context = this;
            
            InjectionBinderCrossContext.BindInstance<GameObject>(_gameObject, GetType().Name);
            InjectionBinder.BindInstance<GameObject>(_gameObject, GetType().Name);
            
            InjectionBinderCrossContext.BindMonoBehaviorInstance<IUpdateProvider, UpdateProvider>();
            InjectionBinderCrossContext.BindMonoBehaviorInstance<ICoroutineProvider, CoroutineProvider>();
        }

        public virtual void SignalBindings(){}
        public virtual void InjectionBindings(){}
        public virtual void MediationBindings(){}
        public virtual void CommandBindings(){}
        public virtual void PostBindings() { }
        public virtual void Setup() { }
        public virtual void Launch() { }

        public virtual void DestroyContext()
        {
            ContextStarted = false;
            
            MediationBinder?.UnBindAll();
            InjectionBinder?.UnBindAll();
            CommandBinder?.UnBindAll();
        }
    }
}