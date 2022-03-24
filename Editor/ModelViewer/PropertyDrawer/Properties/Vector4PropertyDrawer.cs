using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector4PropertyDrawer : PropertyDrawer<Vector4>
    {
        public Vector4PropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.Vector4Field(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}