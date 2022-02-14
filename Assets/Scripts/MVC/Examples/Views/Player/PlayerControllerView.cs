using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Examples.Views.Player
{
    [RequireComponent(typeof(ViewInjectorComponent))]
    public class PlayerControllerView : MonoBehaviour, IView
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}