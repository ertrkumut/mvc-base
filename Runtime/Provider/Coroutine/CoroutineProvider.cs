using System;
using System.Collections;
using MVC.Runtime.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace MVC.Runtime.Provider.Coroutine
{
    [HideInModelViewer]
    public class CoroutineProvider : MonoBehaviour, ICoroutineProvider
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        #region WaitForSeconds

        public UnityEngine.Coroutine WaitForSeconds(float seconds, UnityAction callback)
        {
            IEnumerator _(float secs, UnityAction call)
            {
                yield return new WaitForSeconds(secs);
            
                call?.Invoke();
            }
            
            return StartCoroutine(_(seconds, callback));
        }

        #endregion

        #region WaitEndOfFrame

        public UnityEngine.Coroutine WaitForEndOfFrame(UnityAction callback)
        {
            IEnumerator _(UnityAction call)
            {
                yield return new WaitForEndOfFrame();
                call?.Invoke();
            }

            return StartCoroutine(_(callback));
        }

        public UnityEngine.Coroutine WaitForEndOfFrames(int frameCount, UnityAction callback)
        {
            IEnumerator _(int fC, UnityAction call)
            {
                for(var ii = 0; ii < fC; ii++)
                    yield return new WaitForEndOfFrame();
                
                call?.Invoke();
            }

            
            return StartCoroutine(_(frameCount, callback));
        }

        #endregion

        public UnityEngine.Coroutine WaitUntil(Func<bool> condition, UnityAction callback)
        {
            IEnumerator _(Func<bool> condition, UnityAction call)
            {
                yield return new WaitUntil(condition);
                
                call?.Invoke();
            }

            return StartCoroutine(_(condition, callback));
        }
    }
}