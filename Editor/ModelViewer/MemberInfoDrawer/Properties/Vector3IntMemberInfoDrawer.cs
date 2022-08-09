#if UNITY_EDITOR
using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class Vector3IntMemberInfoDrawer : MemberInfoDrawer<Vector3Int>
    {
        protected override Type _propertyDrawerType => typeof(Vector3IntPropertyDrawer);
        
        public Vector3IntMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}
#endif