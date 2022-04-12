using MVC.Pool;
using UnityEngine;

namespace MVC.Screen.View
{
    public class ScreenBody : MonoBehaviour, IScreenBody
    {
        [SerializeField] private bool _customOpeningAnimation;
        [SerializeField] private bool _customClosingAnimation;

        public ScreenState ScreenState { get; set; }
        
        public bool CustomOpeningAnimation => _customOpeningAnimation;
        public bool CustomClosingAnimation => _customClosingAnimation;

        protected virtual void ScreenOpened()
        {
            ScreenState = ScreenState.InUse;
        }
        
        protected virtual void ScreenClosed()
        {
            ScreenState = ScreenState.InPool;
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

    public interface IScreenBody : IPoolable
    {
        /// <summary>
        /// It shows the state of the screen. None, InPool, InUse, InOpeningAnimation, InClosingAnimation
        /// </summary>
        ScreenState ScreenState { get; set; }

        /// <summary>
        /// It must be true, if there is custom opening animation like Timeline.
        /// If it's true, you need to manual Invoke ScreenOpened method
        /// </summary>
        bool CustomOpeningAnimation { get; }
        
        /// <summary>
        /// It must be true, if there is custom closing animation like Timeline.
        /// If it's true, you need to manual Invoke ScreenClosed method
        /// </summary>
        bool CustomClosingAnimation { get; }
    }
}