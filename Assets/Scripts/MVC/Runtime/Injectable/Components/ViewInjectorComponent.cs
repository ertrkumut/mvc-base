using System.Collections.Generic;
using System.Linq;
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
                if (viewInjectorData.autoInject)
                    TryToInject(viewInjectorData.view as IView);
            }
        }
        
        private bool TryToInject(IView viewComponent)
        {
            var injectorData = GetViewInjectorData(viewComponent);
            if (injectorData.isInjected)
                return false;

            var injectResult = viewComponent.InjectView();
            injectorData.isInjected = injectResult;
            return injectResult;
        }

        private ViewInjectorData GetViewInjectorData(IView view)
        {
            var data = viewDataList.FirstOrDefault(x => Equals(x.view, view));
            return data;
        }

        public void ViewInjectionCompleted(IView view)
        {
            var injectorData = GetViewInjectorData(view);
            if(injectorData == null)
                return;
            
            injectorData.isInjected = true;
        }

        // TEST
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.A))
        //     {
        //         TryToInject(viewDataList[0].view as IMVCView);
        //     }
        // }
    }
}