#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class FloatPropertyDrawer : PropertyDrawer<float>
    {
        public FloatPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            float newValue;
            if(ShowFieldName)
                newValue = EditorGUILayout.FloatField(new GUIContent(_fieldName), propertyValue);
            else
                newValue = EditorGUILayout.FloatField(propertyValue);
            
            if(Math.Abs(newValue - propertyValue) > 0.0001f)
                SetValue(newValue); 
        }
    }
}
#endif