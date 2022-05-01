using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class ListPropertyDrawer<T> : PropertyDrawer<List<T>>
    {
        private bool _foldOut;
        
        private List<PropertyDrawerBase> _enabledProperties;
        private List<PropertyDrawerBase> _disabledProperties;

        private Type _propertyDrawerType;
        
        public ListPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
            _enabledProperties = new List<PropertyDrawerBase>();
            _disabledProperties = new List<PropertyDrawerBase>();
            
            _propertyDrawerType = ModelViewerUtils.GetPropertyDrawerType(typeof(T));
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();
            
            EditorGUILayout.BeginVertical("box");

            _foldOut = EditorGUILayout.Foldout(_foldOut, _fieldName);

            if (!_foldOut)
            {
                EditorGUILayout.EndVertical();
                return;
            }
            
            var value = GetValue();
            if (value == null)
            {
                if (GUILayout.Button("NULL")) 
                    SetValue(new List<T>());

                EditorGUILayout.EndVertical();
                return;
            }

            SendPropertyDrawersToPool();
            
            for (var ii = 0; ii < value.Count; ii++)
            {
                var piece = value[ii];
                PieceGUI(piece, ii);
            }
            
            EditorGUILayout.EndVertical();
        }
        
        private void PieceGUI(T piece, int listIndex)
        {
            var propertyDrawer = GetAvailablePropertyDrawer();
            propertyDrawer.SetFieldName(listIndex.ToString());
            ((PropertyDrawer<T>) propertyDrawer).SetValue(piece);

            propertyDrawer.OnValueChanged += () =>
            {
                var value = GetValue();
                value[listIndex] = ((PropertyDrawer<T>) propertyDrawer).GetValue();
                SetValue(value);
            };

            EditorGUILayout.BeginHorizontal();
            
            propertyDrawer.OnGUI();

            GUI.backgroundColor = Color.red;
            var removeButton = GUILayout.Button("-", GUILayout.Width(15));
            GUI.backgroundColor = Color.white;

            if (removeButton)
            {
                SendPropertyDrawerToPool(propertyDrawer);
                
                var listValue = GetValue();
                listValue.RemoveAt(listIndex);
                SetValue(listValue);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void SendPropertyDrawersToPool()
        {
            for (var ii = 0; ii < _enabledProperties.Count; ii++)
            {
                var propDrawer = _enabledProperties[0];
                SendPropertyDrawerToPool(propDrawer);
            }
        }

        private void SendPropertyDrawerToPool(PropertyDrawerBase propertyDrawerBase)
        {
            propertyDrawerBase.OnValueChanged = null;
                
            _enabledProperties.Remove(propertyDrawerBase);
            _disabledProperties.Add(propertyDrawerBase);
        }
        
        private PropertyDrawerBase GetAvailablePropertyDrawer()
        {
            PropertyDrawerBase availablePropertyDrawer;
            if (_disabledProperties.Count == 0)
            {
                availablePropertyDrawer =
                    (PropertyDrawerBase) Activator.CreateInstance(_propertyDrawerType, _fieldName, _readOnly);
            }
            else
            {
                availablePropertyDrawer = _disabledProperties[0];
                _disabledProperties.Remove(availablePropertyDrawer);
            }
            
            _enabledProperties.Add(availablePropertyDrawer);

            return availablePropertyDrawer;
        }
    }
}