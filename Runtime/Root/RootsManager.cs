using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.Console;
using MVC.Runtime.Bind.Bindings.Pool;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Injectable;
using MVC.Runtime.Injectable.CrossContext;
using MVC.Runtime.Signals;
using MVC.Runtime.ViewMediators.Mediator;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootsManager
    {
        private static RootsManager _instance;
        public static RootsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RootsManager();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        public InjectionBinderCrossContext injectionBinderCrossContext;
        public MediatorCreatorController mediatorCreatorController;
        public BindingPoolController bindingPoolController;

        public Action<IContext> OnContextReady;

        private bool _contextsStarted;
        
        private List<IContextRoot> _contextRootList;
        
        public void Initialize()
        {
            MVCConsole.Log(ConsoleLogType.Context, "RootsManager Created - Initialize Started!");

            _contextRootList = new List<IContextRoot>();
            
            bindingPoolController = new BindingPoolController();
            injectionBinderCrossContext = new InjectionBinderCrossContext();
            mediatorCreatorController = new MediatorCreatorController();
            
            MVCConsole.Log(ConsoleLogType.Context, "RootsManager - Initialize Completed!");
        }

        private void SetSignalsNames()
        {
            SetSignalsNameInInjectionBinder(injectionBinderCrossContext);
            
            foreach (var contextRoot in _contextRootList)
            {
                var context = contextRoot.GetContext();
                SetSignalsNameInInjectionBinder(context.InjectionBinder);
            }
        }

        private void SetSignalsNameInInjectionBinder(InjectionBinder injectionBinder)
        {
            var injectedObjectList = injectionBinder.GetInjectedInstances();

            foreach (var injectionBinding in injectedObjectList)
            {
                var value = injectionBinding.Value;
                var valueType = value.GetType();
                    
                var signalFieldList = valueType
                    .GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => typeof(ISignalBody).IsAssignableFrom(x.FieldType))
                    .ToList();

                foreach (var fieldInfo in signalFieldList)
                {
                    var fieldName = fieldInfo.Name;
                    var signalNameField = fieldInfo
                        .FieldType
                        .GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic);
                    
                    signalNameField.SetValue(fieldInfo.GetValue(value), fieldName);
                }
            }
        }

        public void RegisterContext(IContextRoot contextRoot)
        {
            _contextRootList.Add(contextRoot);
            MVCConsole.Log(ConsoleLogType.Context, "Context Registered! Context: " + contextRoot.GetType().Name);
        }

        public void UnRegisterContext(IContextRoot contextRoot)
        {
            _contextsStarted = false;
            _contextRootList.Remove(contextRoot);
            MVCConsole.Log(ConsoleLogType.Context, "Context Unregistered! Context: " + contextRoot.GetType().Name);
        }

        public void StartContexts()
        {
            if(_contextsStarted)
                return;

            _contextsStarted = true;

            var unreadyContextList = _contextRootList.
                Where(context => !context.GetContext().ContextStarted)
                .OrderBy(x => x.GetContext().InitializeOrder)
                .ToList();
            
            foreach (var contextRoot in unreadyContextList)
            {
                contextRoot.StartContext();
            }

            #if UNITY_EDITOR

                SetSignalsNames();
                        
            #endif
            
            foreach (var contextRoot in _contextRootList)
            {
                contextRoot.Launch();
            }
        }

        public bool IsContextReady(IContext context)
        {
            return _contextRootList
                .FirstOrDefault(contextRoot => contextRoot.GetContext() == context).GetContext()
                .ContextStarted;
        }
    }
}