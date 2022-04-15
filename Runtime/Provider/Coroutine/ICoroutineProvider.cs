using System.Collections;
using UnityEngine.Events;

namespace MVC.Runtime.Provider.Coroutine
{
    public interface ICoroutineProvider
    {
        UnityEngine.Coroutine WaitForSeconds(float seconds, UnityAction callback);

        UnityEngine.Coroutine WaitForEndOfFrame(UnityAction callback);
        UnityEngine.Coroutine WaitForEndOfFrames(int frameCount, UnityAction callback);

        UnityEngine.Coroutine StartCoroutine(IEnumerator enumerator);
        
        void StopCoroutine(UnityEngine.Coroutine coroutine);
        void StopCoroutine(IEnumerator enumerator);
        void StopCoroutine(string methodName);
        void StopAllCoroutines();
    }
}