using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Runtime.Editor.ModelViewer.PropertyDrawer.Properties
{
    public class MVCVector2IntPropertyDrawer : MVCPropertyDrawer<Vector2Int>
    {
        public MVCVector2IntPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
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