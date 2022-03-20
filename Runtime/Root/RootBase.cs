using MVC.Runtime.Contexts;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class RootBase : MonoBehaviour
    {
        public IContext Context { get; set; }
    }
}