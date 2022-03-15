using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Runtime.Editor.ModelViewer.PropertyDrawer.Properties
{
    public class MVCIntPropertyDrawer : MVCPropertyDrawer<int>
    {
        public MVCIntPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.IntField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}