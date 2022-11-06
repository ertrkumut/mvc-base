#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class ObjectPropertyDrawer<TObjectType> : PropertyDrawer<TObjectType>
        where TObjectType : Object
    {
        public ObjectPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }
        
        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            var propertyValue = GetValue();
            TObjectType newValue;
            
            if(ShowFieldName)
                newValue = (TObjectType) EditorGUILayout.ObjectField(new GUIContent(_fieldName), propertyValue, typeof(TObjectType), true);
            else
                newValue = (TObjectType) EditorGUILayout.ObjectField(propertyValue, typeof(TObjectType), true);
            
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}
#endif