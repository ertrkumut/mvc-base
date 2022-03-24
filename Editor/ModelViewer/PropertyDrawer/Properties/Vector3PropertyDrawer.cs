using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector3PropertyDrawer : PropertyDrawer<Vector3>
    {
        public Vector3PropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.Vector3Field(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}