using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector2IntPropertyDrawer : PropertyDrawer<Vector2Int>
    {
        public Vector2IntPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.Vector2IntField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}