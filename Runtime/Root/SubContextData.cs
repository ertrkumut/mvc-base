using System;

namespace MVC.Runtime.Root
{
    [Serializable]
    public struct SubContextData
    {
        public string ContextFullName;
        public string ContextName;

        public bool AutoSetup;
    }
}