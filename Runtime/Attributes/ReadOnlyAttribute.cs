using System;
using UnityEngine;

namespace MVC.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : Attribute
    {
    }
    
#if UNITY_EDITOR
    
    [UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            var wasEnabled = GUI.enabled;
            GUI.enabled = false;

            UnityEditor.EditorGUI.PropertyField(position, property);
            
            GUI.enabled = wasEnabled;
        }
    }
    
#endif
}