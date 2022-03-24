using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class EnumPropertyDrawer : PropertyDrawer<Enum>
    {
        public EnumPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
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