using System;

namespace MVC.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class HideFromModelViewerAttribute : Attribute
    {
        
    }
}