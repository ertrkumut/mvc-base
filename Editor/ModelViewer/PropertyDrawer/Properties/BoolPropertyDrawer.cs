using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class BoolPropertyDrawer : PropertyDrawer<bool>
    {
        public BoolPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }
        
        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.Toggle(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}