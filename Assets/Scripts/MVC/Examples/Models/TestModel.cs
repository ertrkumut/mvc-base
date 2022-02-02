using MVC.Runtime.Injectable.Attributes;
using UnityEngine;

namespace MVC.Examples.Models
{
    public class TestModel : ITestModel
    {
        [PostConstruct]
        private void PostConstruct()
        {
            Debug.Log("Test model PostConstruct");
        }
    }

    public interface ITestModel
    {
        
    }
}