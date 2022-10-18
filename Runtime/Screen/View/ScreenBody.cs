using System;
using MVC.Runtime.Screen.Enum;
using UnityEngine;

namespace MVC.Runtime.Screen.View
{
    public class ScreenBody : MonoBehaviour, IScreenBody
    {
        public Action OnScreenOpened;
        public Action OnScreenClosed;
        
        [SerializeField] private bool _customOpeningAnimation;
        [SerializeField] private bool _customClosingAnimation;

        public ScreenState ScreenState { get; set; }
        public System.Enum ScreenType { get; set; }
        public ScreenLayerIndex LayerIndex { get; set; }

        public int ScreenManagerId { get; set; }
        
        public bool CustomOpeningAnimation => _customOpeningAnimation;
        public bool CustomClosingAnimation => _customClosingAnimation;

        protected virtual void ScreenOpened()
        {
            ScreenState = ScreenState.InUse;
            OnScreenOpened?.Invoke();
        }
        
        protected virtual void ScreenClosed()
        {
            ScreenState = ScreenState.InPool;
            gameObject.SetActive(false);
            OnScreenClosed?.Invoke();
            OnReturnToPool();
        }

        // It runs by ScreenModel
        internal void Open()
        {
            gameObject.SetActive(true);
            OnGetFromPool();
            
            if(!_customOpeningAnimation)
            {
                ScreenOpened();
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
                ScreenClosed();
                gameObject.SetActive(false);
                ScreenState = ScreenState.InPool;
                return;
            }
            
            ScreenState = ScreenState.InClosingAnimation;
            ClosingAnimation();
        }

        internal virtual void InitializeScreenParams(params object[] screenParams)
        {
        }

        public string PoolKey { get; set; }

        public virtual void OnGetFromPool()
        {
        }

        public virtual void OnReturnToPool()
        {
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
    }
}