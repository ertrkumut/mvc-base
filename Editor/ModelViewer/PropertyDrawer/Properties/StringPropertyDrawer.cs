#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class StringPropertyDrawer : PropertyDrawer<string>
    {
        public StringPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            string newValue;
            if (ShowFieldName)
                newValue = EditorGUILayout.TextField(new GUIContent(_fieldName), propertyValue);
            else
                newValue = EditorGUILayout.TextField(propertyValue);
            
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}
#endif