using MVC.Runtime.Controller;
using UnityEngine;

namespace MVC.Examples.Controller
{
    public class TestCommand1 : Command
    {
        public override void Execute()
        {
            Debug.Log("Command 1");
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