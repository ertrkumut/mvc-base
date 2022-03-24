using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class StringPropertyDrawer : PropertyDrawer<string>
    {
        public StringPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.TextField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}