using System.Collections.Generic;
using MVC.Runtime.Attributes;

namespace MVC.Runtime.Injectable.CrossContext
{
    [HideInModelViewer]
    public class InjectionBinderCrossContext : InjectionBinder
    {
        internal List<object> PostConstructedObjects;

        public InjectionBinderCrossContext()
        {
            PostConstructedObjects = new List<object>();
        }
    }
}