using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class Vector3IntPropertyDrawer : PropertyDrawer<Vector3Int>
    {
        public Vector3IntPropertyDrawer(Vector3Int property, string fieldName, bool readOnly) : base(property, fieldName, readOnly)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.Vector3IntField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}