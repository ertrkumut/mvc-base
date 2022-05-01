using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class IntMemberInfoDrawer : MemberInfoDrawer<int>
    {
        protected override Type _propertyDrawerType => typeof(IntPropertyDrawer);
        
        public IntMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}