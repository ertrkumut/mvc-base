﻿using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class Vector3IntMemberInfoDrawer : MemberInfoDrawer<Vector3Int>
    {
        protected override Type _propertyDrawerType => typeof(Vector3IntPropertyDrawer);
        
        public Vector3IntMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.Vector3IntField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}