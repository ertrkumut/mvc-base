using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class Vector4MemberInfoDrawer : MemberInfoDrawer<Vector4>
    {
        protected override Type _propertyDrawerType => typeof(Vector4PropertyDrawer);
        
        public Vector4MemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}