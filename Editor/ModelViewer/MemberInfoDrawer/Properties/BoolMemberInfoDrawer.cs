using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class BoolMemberInfoDrawer : MemberInfoDrawer<bool>
    {
        protected override Type _propertyDrawerType => typeof(BoolPropertyDrawer);

        public BoolMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}