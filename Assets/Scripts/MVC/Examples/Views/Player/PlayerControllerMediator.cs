using MVC.Examples.Contexts;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;
using UnityEngine;

namespace MVC.Examples.Views.Player
{
    public class PlayerControllerMediator : MonoBehaviour, IMVCMediator
    {
        [Inject] private PlayerControllerView _view { get; set; }
        [Inject] private TestClass _testClass;
        
        public void OnRegister()
        {
            Debug.Log("OnRegister");
        }

        public void OnRemove()
        {
            Debug.Log("OnRemove");
        }
    }
}