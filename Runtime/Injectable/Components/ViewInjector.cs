﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MVC.Runtime.Contexts;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Utils;
using MVC.Runtime.ViewMediators.View;
using MVC.Runtime.ViewMediators.View.Data;
using UnityEngine;

namespace MVC.Runtime.Injectable.Components
{
    public class ViewInjector : MonoBehaviour
    {
        public List<ViewInjectorData> viewDataList;

        private Dictionary<IView, IContext> _viewRegistrationDataDict;
        
        #region Unity Methods

        private void Start()
        {
            InjectViews();
        }

        protected virtual void OnDestroy()
        {
            RootsManager.Instance.OnContextReady -= OnContextsReadyListener;
            
            for (var ii = 0; ii < viewDataList.Count; ii++)
            {
                var viewInjectorData = viewDataList[ii];
                if(!viewInjectorData.IsRegistered)
                    continue;
                var view = viewInjectorData.View as IView;
                view.UnRegister();
            }
        }

        #endregion

        #region Injection

        public void InjectViews()
        {
            _viewRegistrationDataDict = new Dictionary<IView, IContext>();
            
            IContext bubbleUpContext = null;
            var rootsManager = RootsManager.Instance;

            if (viewDataList == null)
            {
                viewDataList = new();
                var viewEntities = GetComponents<IView>().ToList();

                foreach (var viewEntity in viewEntities)
                {
                    var injectorData = new ViewInjectorData
                    {
                        View = viewEntity as Object,
                        AutoRegister = true
                    };
                    viewDataList.Add(injectorData);
                }
            }
            
            foreach (var viewInjectorData in viewDataList)
            {
                if(viewInjectorData.SelectedRoot == null && bubbleUpContext == null)
                    bubbleUpContext = (transform.GetComponent<IView>()).FindViewContext();
                
                if (!viewInjectorData.UseBubbleUp && viewInjectorData.SelectedRoot != null)
                {
                    var context = viewInjectorData.SelectedRoot.GetContext();
                    _viewRegistrationDataDict.Add(viewInjectorData.View as IView, context);
                    
                    if (rootsManager.IsContextReady(context))
                        RegisterView(viewInjectorData);
                    else
                        rootsManager.OnContextReady += OnContextsReadyListener;
                }
                else
                {
                    _viewRegistrationDataDict.Add(viewInjectorData.View as IView, bubbleUpContext);
                    
                    if (rootsManager.IsContextReady(bubbleUpContext))
                        RegisterView(viewInjectorData);
                    else
                        rootsManager.OnContextReady += OnContextsReadyListener;
                }
            }
        }

        public void UnregisterViews()
        {
            foreach (var viewInjectorData in viewDataList)
            {
                (viewInjectorData.View as IView).UnRegister();
            }
        }
        
        private void OnContextsReadyListener(IContext context)
        {
            foreach (var (key, value) in _viewRegistrationDataDict)
            {
                if(value != context)
                    continue;
                
                var view = key;
                if(view == null)
                    return;

                RootsManager.Instance.OnContextReady -= OnContextsReadyListener;
            
                var viewInjectorData = viewDataList.FirstOrDefault(x => x.View == (Object) view && !x.IsRegistered);
                if(viewInjectorData is not null)
                    RegisterView(viewInjectorData);
            }
        }

        private void RegisterView(ViewInjectorData viewInjectorData)
        {
            if (viewInjectorData.AutoRegister)
                TryToInject(viewInjectorData.View as IView);
        }
        
        private bool TryToInject(IView viewComponent)
        {
            var injectorData = GetViewInjectorData(viewComponent);
            if (injectorData.IsRegistered)
                return false;

            var injectResult = viewComponent.Register(injectorData);
            injectorData.IsRegistered = injectResult;
            return injectResult;
        }

        public ViewInjectorData GetViewInjectorData(IView view)
        {
            var data = viewDataList.FirstOrDefault(x => Equals(x.View, view));
            return data;
        }

        public void ViewInjectionCompleted(IView view)
        {
            var injectorData = GetViewInjectorData(view);
            if(injectorData == null)
                return;
            
            injectorData.IsRegistered = true;
            view.IsRegistered = true;
        }

        #endregion

        internal void InitializeForEditor()
        {
            viewDataList = new List<ViewInjectorData>();

            var viewComponentList = GetComponents<IView>().ToList();

            foreach (var viewComponent in viewComponentList)
            {
                var viewInjectorData = new ViewInjectorData
                {
                    View = viewComponent as Object,
                    AutoRegister = true,
                    IsRegistered = false
                };
                
                viewDataList.Add(viewInjectorData);
            }
        }
    }
}