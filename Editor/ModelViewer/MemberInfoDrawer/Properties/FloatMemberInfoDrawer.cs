using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class FloatMemberInfoDrawer : MemberInfoDrawer<float>
    {
        protected override Type _propertyDrawerType => typeof(FloatPropertyDrawer);
        
        public FloatMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}