using MVC.Examples.Signals;
using MVC.Runtime.Function;
using MVC.Runtime.Function.Provider;
using MVC.Runtime.Function.ReturnableFunctions;
using MVC.Runtime.Function.VoidFunctions;
using MVC.Runtime.Injectable.Attributes;
using UnityEngine;

namespace MVC.Examples.Functions
{
    public class TestFunction : FunctionVoid
    {
        public override void Execute()
        {
            Debug.Log("Test Function Executed");
        }
    }
    
    public class MathFunction : FunctionReturn<float, int, int>
    {
        [Inject] private GameSignals _gameSignals { get; set; }
        [Inject] private IFunctionProvider _functionProvider;
        
        public override float Execute(int param1, int param2)
        {
            var result =  param1 + param2;
            Debug.Log("Math Function Executed! - Result: " + result);
            return result;
        }
    }
}