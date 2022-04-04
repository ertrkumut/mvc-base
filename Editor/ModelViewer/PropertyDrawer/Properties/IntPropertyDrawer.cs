using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class IntPropertyDrawer : PropertyDrawer<int>
    {
        public IntPropertyDrawer(int property, string fieldName, bool readOnly) : base(property, fieldName, readOnly)
        {
        }

        public override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            var newValue = EditorGUILayout.IntField(new GUIContent(_fieldName), propertyValue);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}