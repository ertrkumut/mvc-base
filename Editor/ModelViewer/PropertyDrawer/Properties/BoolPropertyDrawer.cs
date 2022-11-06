#if UNITY_EDITOR
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
            bool newValue = false;
            if (ShowFieldName)
                newValue = EditorGUILayout.Toggle(new GUIContent(_fieldName), propertyValue);
            else
                newValue = EditorGUILayout.Toggle(propertyValue);
            
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}
#endif