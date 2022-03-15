using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Runtime.Editor.ModelViewer.PropertyDrawer.Properties
{
    public class MVCEnumPropertyDrawer : MVCPropertyDrawer<Enum>
    {
        public MVCEnumPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
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