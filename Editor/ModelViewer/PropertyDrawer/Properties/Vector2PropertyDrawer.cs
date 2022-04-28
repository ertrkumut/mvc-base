using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector2PropertyDrawer : PropertyDrawer<Vector2>
    {
        public Vector2PropertyDrawer(Vector2 property, string fieldName, bool readOnly) : base(property, fieldName, readOnly)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.Vector2Field(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}