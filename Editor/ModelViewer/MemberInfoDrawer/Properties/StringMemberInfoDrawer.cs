#if UNITY_EDITOR
using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class StringMemberInfoDrawer : MemberInfoDrawer<string>
    {
        protected override Type _propertyDrawerType => typeof(StringPropertyDrawer);
        
        public StringMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}
#endif