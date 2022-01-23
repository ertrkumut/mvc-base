using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Utils;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Components
{
    public class ViewInjectorComponent : MonoBehaviour
    {
        private List<IMVCView> _viewComponents;
        
        private void Awake()
        {
            FindViews();
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

        private void FindViews()
        {
            _viewComponents = GetComponents<IMVCView>().ToList();
        }

        private void RegisterViews()
        {
            foreach (var viewComponent in _viewComponents)
            {
                InjectMediator(viewComponent);
            }
        }
        
        private void InjectMediator(IMVCView viewComponent)
        {
            viewComponent.InitializeView();
        }
    }
}