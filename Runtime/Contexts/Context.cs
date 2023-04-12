using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Console;
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
            InjectionBinderCrossContext.SetContext(this);
            SubContexts = subContexts;

            AllContexts = subContexts;
            AllContexts.Insert(0, this);
        }

        public void Start()
        {
            ContextStarted = true;
            
            CoreBindings();
        }

        void IContext.InjectAllInstances()
        {
            var injectedBindings = InjectionBinder.GetInjectedInstances();
            var injectedCrossContextBindings = InjectionBinderCrossContext.GetInjectedInstances();

            injectedBindings = injectedBindings.Concat(injectedCrossContextBindings).ToList();

            foreach (InjectionBinding binding in injectedBindings)
            {
                if(binding == null)
                    continue;

                if (binding.ContainerContext == null)
                    this.TryToInjectObject(binding.Value);
                else
                    binding.ContainerContext.TryToInjectObject(binding.Value);
            }
            
        }
        
        void IContext.ExecutePostConstructMethods()
        {
            var injectedTypes = InjectionBinder.GetInjectedInstances();
            var injectedCrossContextTypes = InjectionBinderCrossContext.GetInjectedInstances();

            injectedTypes = injectedTypes.Concat(injectedCrossContextTypes).ToList();
            
            foreach (InjectionBinding injectedType in injectedTypes)
            {
                if(InjectionBinderCrossContext.PostConstructedObjects.Contains(injectedType.Value))
                    continue;
                
                PostConstructUtils.ExecutePostConstructMethod(injectedType.Value);
                
                InjectionBinderCrossContext.PostConstructedObjects.Add(injectedType.Value);
            }
            
            MVCConsole.Log(ConsoleLogType.Context, "Context Executed Post Construct Methods! Context: " + GetType().Name);
        }

        protected virtual void CoreBindings()
        {
            InjectionBinder = new InjectionBinder();
            //InjectionBinder.SetContext(this);
            MediationBinder = InjectionBinder.Bind<MediationBinder>();
            
            InjectionBinderCrossContext.BindInstance(InjectionBinderCrossContext);

            CommandBinder = InjectionBinder.Bind<ICommandBinder, CommandBinder>();
            ((CommandBinder) CommandBinder).Context = this;

            var functionProvider = (FunctionProvider) InjectionBinder.Bind<IFunctionProvider, FunctionProvider>();
            functionProvider.Context = this;
            
            InjectionBinderCrossContext.BindInstance<GameObject>(_gameObject, GetType().Name);
            InjectionBinder.BindInstance<GameObject>(_gameObject, nameof(IContext));
            
            InjectionBinderCrossContext.BindMonoBehaviorInstance<IUpdateProvider, UpdateProvider>();
            InjectionBinderCrossContext.BindMonoBehaviorInstance<ICoroutineProvider, CoroutineProvider>();
        }

        public virtual void SignalBindings(){}
        public virtual void InjectionBindings(){}
        public virtual void MediationBindings(){}
        public virtual void CommandBindings(){}
        
        public virtual void PostBindings()
        {
            
        }
        
        public virtual void Launch()
        {
        }

        public virtual void DestroyContext()
        {
            ContextStarted = false;
            
            MediationBinder?.UnBindAll();
            InjectionBinder?.UnBindAll();
        }
    }
}