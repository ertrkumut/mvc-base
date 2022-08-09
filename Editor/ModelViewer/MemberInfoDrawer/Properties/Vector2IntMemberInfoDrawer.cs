#if UNITY_EDITOR
using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class Vector2IntMemberInfoDrawer : MemberInfoDrawer<Vector2Int>
    {
        protected override Type _propertyDrawerType => typeof(Vector2IntPropertyDrawer);
        
        public Vector2IntMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}
#endif