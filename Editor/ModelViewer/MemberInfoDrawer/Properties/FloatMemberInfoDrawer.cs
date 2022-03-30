using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class FloatMemberInfoDrawer : MemberInfoDrawer<float>
    {
        public FloatMemberInfoDrawer(FieldInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.FloatField(new GUIContent(_fieldName), propertyValue);
            if(Math.Abs(newValue - propertyValue) > 0.0001f)
                SetValue(newValue);
        }
    }
}