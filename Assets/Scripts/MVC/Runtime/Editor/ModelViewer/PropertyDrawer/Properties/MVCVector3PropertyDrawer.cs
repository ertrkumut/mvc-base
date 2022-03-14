using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Packages.Editor.ModelViewer.PropertyDrawer.Properties
{
    public class MVCVector3PropertyDrawer : MVCPropertyDrawer<Vector3>
    {
        public MVCVector3PropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
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