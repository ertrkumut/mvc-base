using System;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class FloatPropertyDrawer : PropertyDrawer<float>
    {
        public FloatPropertyDrawer(float property, string fieldName, bool readOnly) : base(property, fieldName, readOnly)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.FloatField(new GUIContent(_fieldName), propertyValue);
            if(Math.Abs(newValue - propertyValue) > 0.0001f)
                SetValue(newValue); 
        }
    }
}