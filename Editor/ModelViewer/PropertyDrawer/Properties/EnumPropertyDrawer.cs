#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class EnumPropertyDrawer : PropertyDrawer<Enum>
    {
        public EnumPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            if (propertyValue == null)
            {
                EditorGUILayout.LabelField(new GUIContent(_fieldName + ": null"));
                return;
            }
            Enum newValue;
            
            if (ShowFieldName)
                newValue = EditorGUILayout.EnumPopup(new GUIContent(_fieldName), propertyValue);
            else
                newValue = EditorGUILayout.EnumPopup(propertyValue);
            
            if(!Equals(newValue, propertyValue))
                SetValue(newValue);
        }
    }
}
#endif