using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class Vector3MemberInfoDrawer : MemberInfoDrawer<Vector3>
    {
        protected override Type _propertyDrawerType => typeof(Vector3PropertyDrawer);
        
        public Vector3MemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}