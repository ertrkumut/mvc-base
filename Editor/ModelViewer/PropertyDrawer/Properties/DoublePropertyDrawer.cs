#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class DoublePropertyDrawer : PropertyDrawer<double>
    {
        public DoublePropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }
        
        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            double newValue;
            if (ShowFieldName)
                newValue = EditorGUILayout.DoubleField(new GUIContent(_fieldName), propertyValue);
            else
                newValue = EditorGUILayout.DoubleField(propertyValue);
            
            if(Math.Abs(newValue - propertyValue) > 0.001d)
                SetValue(newValue);
        }
    }
}
#endif