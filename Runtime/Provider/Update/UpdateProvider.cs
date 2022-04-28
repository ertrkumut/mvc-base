using System;
using MVC.Runtime.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace MVC.Runtime.Provider.Update
{
    [HideInModelViewer]
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        private event UnityAction OnUpdate;
        private event UnityAction OnLateUpdate;
        private event UnityAction OnFixedUpdate;

        #region Update

        public void AddUpdate(UnityAction callback)
        {
            OnUpdate += callback;
        }

        public void RemoveUpdate(UnityAction callback)
        {
            OnUpdate -= callback;
        }

        #endregion

        #region LateUpdate

        public void AddLateUpdate(UnityAction callback)
        {
            OnLateUpdate += callback;
        }

        public void RemoveLateUpdate(UnityAction callback)
        {
            OnLateUpdate -= callback;
        }

        #endregion

        #region FixedUpdate

        public void AddFixedUpdate(UnityAction callback)
        {
            OnFixedUpdate += callback;
        }

        public void RemoveFixedUpdate(UnityAction callback)
        {
            OnFixedUpdate -= callback;
        }

        #endregion

        #region UnityCallback

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        #endregion
    }
}