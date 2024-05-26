using System;

namespace MVC.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SubContextAttribute : Attribute
    {
        public Type ContextType;
        public bool AutoSetup;
    }
}