using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Packages.Editor.ModelViewer.PropertyDrawer.Properties
{
    public class MVCVector3IntPropertyDrawer : MVCPropertyDrawer<Vector3Int>
    {
        public MVCVector3IntPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.Vector3IntField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}