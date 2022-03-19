using MVC.Examples.Entity;
using MVC.Examples.Functions;
using MVC.Examples.Models;
using MVC.Examples.Signals;
using MVC.Runtime.Function.Provider;
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

        [Inject] private GameSignals _gameSignals { get; set; }
        [Inject] private IFunctionProvider _functionProvider;
        
        public override void OnRegister()
        {
            base.OnRegister();

            _view.OnSpaceKeyClicked += SpaceKeyClickedListener;
            Debug.Log("OnRegister");
        }

        public override void OnRemove()
        {
            _view.OnSpaceKeyClicked -= SpaceKeyClickedListener;
            
            Debug.Log("OnRemove");
        }

        private void SpaceKeyClickedListener()
        {
            // _gameSignals.IntTestSignal.Dispatch(20);
            
            // _functionProvider.Execute<TestFunction>()
            //     .SetVoid();

            var result = _functionProvider.Execute<MathFunction>()
                .AddParams(15, 25)
                .SetReturn<float>();
        }
    }

    public class MediatorBaseTest : IMediator
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