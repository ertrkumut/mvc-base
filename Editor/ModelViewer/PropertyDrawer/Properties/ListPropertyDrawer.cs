using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class ListPropertyDrawer<T> : PropertyDrawer<List<T>>
    {
        private List<PropertyDrawerBase> _drawersList;
        private bool _foldOut;
        
        public ListPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
            _drawersList = new List<PropertyDrawerBase>();
        }

        public override void OnDrawGUI()
        {
            _foldOut = EditorGUILayout.Foldout(_foldOut, _fieldInfo.Name);
            
            if(!_foldOut)
                return;

            base.OnDrawGUI();

            var value = GetPropertyValue();
            if (value == null)
            {
                GUI.backgroundColor = Color.red;
                EditorGUILayout.LabelField("NULL");
                GUI.backgroundColor = Color.white;

                if (GUILayout.Button("New List")) SetValue(new List<T>());

                return;
            }

            for (var ii = 0; ii < value.Count; ii++)
            {
            }
        }
    }
}