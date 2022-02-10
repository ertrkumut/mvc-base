using MVC.Examples.Entity;
using MVC.Examples.Models;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;
using UnityEngine;

namespace MVC.Examples.Views.Player
{
    public class PlayerControllerMediator : MediatorBaseTest
    {
        [Inject] private PlayerControllerView _view { get; set; }
        [Inject(Name = "Test")] private TestClass _testClass;
        [Inject(Name = "Test2")] private TestClass _testClass2;
        [Inject] private ITestModel _testModel;
        
        public override void OnRegister()
        {
            base.OnRegister();
            Debug.Log("OnRegister");
        }

        public override void OnRemove()
        {
            Debug.Log("OnRemove");
        }
    }

    public class MediatorBaseTest : MonoBehaviour, IMVCMediator
    {
        [Inject] private ITestModel _testModel;
        
        public virtual void OnRegister()
        {
            Debug.Log("Base mediator Registered");
        }

        public virtual void OnRemove()
        {
        }
    }
}