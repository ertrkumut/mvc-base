using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector2PropertyDrawer : PropertyDrawer<Vector2>
    {
        public Vector2PropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.Vector2Field(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}