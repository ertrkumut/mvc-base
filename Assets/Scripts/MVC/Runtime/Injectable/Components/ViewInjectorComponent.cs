using System.Collections.Generic;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Utils;
using MVC.Runtime.ViewMediators.View;
using MVC.Runtime.ViewMediators.View.Data;
using UnityEngine;

namespace MVC.Runtime.Injectable.Components
{
    public class ViewInjectorComponent : MonoBehaviour
    {
        public List<ViewInjectorData> viewDataList;
        
        private void Awake()
        {
            var rootsManager = RootsManager.Instance;
            if (rootsManager.ContextsReady)
                RegisterViews();
            else
                rootsManager.OnContextsReady += OnContextsReadyListener;
        }

        private void OnContextsReadyListener()
        {
            RootsManager.Instance.OnContextsReady -= OnContextsReadyListener;
            RegisterViews();
        }

        private void RegisterViews()
        {
            foreach (var viewInjectorData in viewDataList)
            {
                if(viewInjectorData.autoInject)
                    viewInjectorData.isInjected = InjectMediator(viewInjectorData.view as IMVCView);
            }
        }
        
        private bool InjectMediator(IMVCView viewComponent)
        {
            return viewComponent.InitializeView();
        }
    }
}