using System;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Pool;
using MVC.Runtime.Screen.Enum;
using MVC.Runtime.ViewMediators.Utils;
using UnityEngine;

namespace MVC.Runtime.Screen.View
{
    public class ScreenBody : MonoBehaviour, IScreenBody
    {
        public bool IsRegistered { get; set; }
        
        public Action ScreenOpened;
        public Action ScreenClosed;
        
        [SerializeField] private bool _customOpeningAnimation;
        [SerializeField] private bool _customClosingAnimation;

        public ScreenState ScreenState { get; set; }
        public System.Enum ScreenType { get; set; }
        public ScreenLayerIndex LayerIndex { get; set; }

        public int ScreenManagerId { get; set; }
        
        public bool CustomOpeningAnimation => _customOpeningAnimation;
        public bool CustomClosingAnimation => _customClosingAnimation;

        // It runs by ScreenModel
        internal void Open()
        {
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            OnGetFromPool();
            
            if(!_customOpeningAnimation)
            {
                OpenScreen();
                return;
            }
            
            ScreenState = ScreenState.InOpeningAnimation;
            OpeningAnimation();
        }

        // It runs by ScreenModel
        internal void Close()
        {
            if (!_customClosingAnimation)
            {
                CloseScreen();
                return;
            }
            
            ScreenState = ScreenState.InClosingAnimation;
            MVCConsole.Log(ConsoleLogType.Screen, "Screen ClosingAnimation started! id: " + this.GetType().Name);
            ClosingAnimation();
        }

        private void OpenScreen()
        {
            ScreenState = ScreenState.InUse;
            ScreenOpened?.Invoke();
        }

        private void CloseScreen()
        {
            ScreenState = ScreenState.InPool;
            gameObject.SetActive(false);
            this.UnRegister();
            (this as IPoolable).ReturnToPool();
            
            MVCConsole.LogWarning(ConsoleLogType.Screen, "Hide Screen! type: " + this.GetType().Name);
            ScreenClosed?.Invoke();
        }

        /// <summary>
        /// It runs if CustomOpeningAnimation is true.
        /// This is the method for handling custom animations.
        /// You can run your timeline animations or you can use tween animations.
        /// </summary>
        protected virtual void OpeningAnimation()
        {
        }

        /// <summary>
        /// It runs if CustomClosingAnimation is true.
        /// This is the method for handling custom animations.
        /// You can run your timeline animations or you can use tween animations.
        /// </summary>
        protected virtual void ClosingAnimation()
        {
        }

        protected void OpeningAnimationCompleted()
        {
            MVCConsole.Log(ConsoleLogType.Screen, "Screen OpeningAnimation completed! id: " + this.GetType().Name);
            OpenScreen();
        }
        protected void ClosingAnimationCompleted()
        {
            CloseScreen();
        }

        internal virtual void InitializeScreenParams(params object[] screenParams)
        {
        }

        #region IPoolable Methods

        public string PoolKey { get; set; }

        public virtual void OnGetFromPool()
        {
        }

        public virtual void OnReturnToPool()
        {
        }

        public Action<IPoolable> ReturnToPoolAction { get; set; }
        #endregion
    }
}