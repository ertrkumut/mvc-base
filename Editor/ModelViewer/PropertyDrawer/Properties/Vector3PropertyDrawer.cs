using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector3PropertyDrawer : PropertyDrawer<Vector3>
    {
        public Vector3PropertyDrawer(Vector3 property, string fieldName, bool readOnly) : base(property, fieldName, readOnly)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.Vector3Field(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}