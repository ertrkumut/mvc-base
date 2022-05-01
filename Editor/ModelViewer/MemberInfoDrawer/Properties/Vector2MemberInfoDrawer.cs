using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class Vector2MemberInfoDrawer : MemberInfoDrawer<Vector2>
    {
        protected override Type _propertyDrawerType => typeof(Vector2PropertyDrawer);
        
        public Vector2MemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}