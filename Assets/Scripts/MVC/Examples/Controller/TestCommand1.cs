using MVC.Runtime.Controller;
using UnityEngine;

namespace MVC.Examples.Controller
{
    public class TestCommand1 : Command
    {
        public override void Execute()
        {
            RetainCommand();
            Debug.Log("Command 1");
            
            ReleaseCommand();
        }
    }
    
    public class TestCommand2 : Command
    {
        public override void Execute()
        {
            Debug.Log("Command 2");
        }
    }
}