using System;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Examples.Views.Player
{
    [RequireComponent(typeof(ViewInjectorComponent))]
    public class PlayerControllerView : MonoBehaviour, IView
    {
        public Action OnSpaceKeyClicked;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpaceKeyClicked?.Invoke();
            }
        }
    }
}