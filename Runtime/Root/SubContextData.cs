using System;

namespace MVC.Runtime.Root
{
    [Serializable]
    public class SubContextData
    {
        public string ContextFullName;
        public string ContextName;

        public bool AutoSetup;
    }
}