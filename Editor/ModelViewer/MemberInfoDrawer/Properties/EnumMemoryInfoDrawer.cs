using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class EnumMemberInfoDrawer : MemberInfoDrawer<Enum>
    {
        public EnumMemberInfoDrawer(FieldInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.EnumPopup(new GUIContent(_fieldName), propertyValue);
            if(!Equals(newValue, propertyValue))
                SetValue(newValue);
        }
    }
}