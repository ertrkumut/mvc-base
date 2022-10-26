using System.Collections.Generic;
using System.Linq;
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

        private IContext _context;
        
        #region Unity Methods

        private void Start()
        {
            _context = (transform.GetComponent<IView>()).FindViewContext();
            if (_context == null)
                return;
            
            var rootsManager = RootsManager.Instance;
            if (rootsManager.IsContextReady(_context))
                RegisterViews();
            else
                rootsManager.OnContextReady += OnContextsReadyListener;
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
                view.UnRegistration();
            }
        }

        #endregion

        #region Injection

        private void OnContextsReadyListener(IContext context)
        {
            if (!_context.Equals(context))
                return;

            RootsManager.Instance.OnContextReady -= OnContextsReadyListener;
            RegisterViews();
        }

        private void RegisterViews()
        {
            foreach (var viewInjectorData in viewDataList)
            {
                if (viewInjectorData.AutoRegister)
                    TryToInject(viewInjectorData.View as IView);
            }
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