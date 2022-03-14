using MVC.Examples.Entity;
using MVC.Runtime.Controller;
using MVC.Runtime.Injectable.Attributes;
using UnityEngine;

namespace MVC.Examples.Controller
{
    public class TestCommand1 : Command
    {
        [Inject] private TestClass _testClass;

        [SignalParam] private int _intParam;
        
        public override void Execute()
        {
            RetainCommand();
            Debug.Log("Command 1 " + _intParam);
            
            ReleaseCommand();
        }
    }
    
    public class TestCommand2 : Command
    {
        [SignalParam] private int _intParam;
        
        public override void Execute()
        {
            RetainCommand();
            Debug.Log("Command 2 " + _intParam);
            ReleaseCommand("Hello World", 15.0f);
        }
    }

    public class TestCommand3 : Command<string, float>
    {
        [SignalParam] private int _intParam;
        
        public override void Execute(string param1, float param2)
        {
            Debug.Log("Command 3 : " + param1 + " - " + param2 + " signalParam: " + _intParam);
        }
    }
}