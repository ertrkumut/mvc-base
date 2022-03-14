using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Packages.Editor.ModelViewer.PropertyDrawer.Properties
{
    public class MVCVector4PropertyDrawer : MVCPropertyDrawer<Vector4>
    {
        public MVCVector4PropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
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