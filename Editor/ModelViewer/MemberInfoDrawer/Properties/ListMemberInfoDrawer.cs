#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class ListMemberInfoDrawer<T> : MemberInfoDrawer<List<T>>
    {
        protected override Type _propertyDrawerType => typeof(ListPropertyDrawer<T>);

        public ListMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}
#endif