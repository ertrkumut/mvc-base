#if UNITY_EDITOR
using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class ObjectMemberInfoDrawer : MemberInfoDrawer<Object>
    {
        protected override Type _propertyDrawerType => typeof(ObjectPropertyDrawer<Object>);
        
        public ObjectMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}
#endif