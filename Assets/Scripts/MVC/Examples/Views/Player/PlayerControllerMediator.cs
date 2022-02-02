using MVC.Examples.Entity;
using MVC.Examples.Models;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;
using UnityEngine;

namespace MVC.Examples.Views.Player
{
    public class PlayerControllerMediator : MonoBehaviour, IMVCMediator
    {
        [Inject] private PlayerControllerView _view { get; set; }
        [Inject] private TestClass _testClass;
        [Inject] private ITestModel _testModel;
        
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