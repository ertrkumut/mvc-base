using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class StringPropertyDrawer : PropertyDrawer<string>
    {
        public StringPropertyDrawer(string property, string fieldName, bool readOnly) : base(property, fieldName, readOnly)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.TextField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}