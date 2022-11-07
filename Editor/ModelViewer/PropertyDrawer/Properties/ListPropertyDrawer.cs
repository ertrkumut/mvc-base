#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class ListPropertyDrawer<T> : PropertyDrawer<List<T>>
    {
        public bool UseFoldOut;
        public bool CanDeleteItem;
        
        private bool _foldOut;
        
        private List<PropertyDrawerBase> _enabledProperties;
        private List<PropertyDrawerBase> _disabledProperties;

        private Type _propertyDrawerType;

        public Action<int, T> ItemValueChanged;

        public ListPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
            _enabledProperties = new List<PropertyDrawerBase>();
            _disabledProperties = new List<PropertyDrawerBase>();
            
            _propertyDrawerType = ModelViewerUtils.GetPropertyDrawerType(typeof(T));
            
            UseFoldOut = true;
        }

        protected override void OnBeforeDrawGUI()
        {
            base.OnBeforeDrawGUI();
            EditorGUILayout.BeginVertical();
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();

            if (_propertyDrawerType == null)
            {
                EditorGUILayout.HelpBox(new GUIContent($"{_fieldName}: Type is not supported!"));
                return;
            }
            
            if(UseFoldOut)
                _foldOut = EditorGUILayout.Foldout(_foldOut, _fieldName);

            if (!_foldOut && UseFoldOut)
                return;
            
            var value = GetValue();
            if (value == null)
            {
                if (GUILayout.Button("NULL")) 
                    SetValue(new List<T>());
                
                return;
            }

            CheckValueCountAndPropertyDrawerCount();
            
            for (var ii = 0; ii < value.Count; ii++)
            {
                var piece = value[ii];
                PieceGUI(piece, ii);
            }
        }

        protected override void OnDrawCompletedGUI()
        {
            base.OnDrawCompletedGUI();
            
            EditorGUILayout.EndVertical();
        }

        private void CheckValueCountAndPropertyDrawerCount()
        {
            var value = GetValue();
            if(value.Count == _enabledProperties.Count)
                return;

            for (var ii = 0; ii < value.Count; ii++)
            {
                if (ii >= _enabledProperties.Count)
                    GetAvailablePropertyDrawer();
                else
                    SendPropertyDrawerToPool(_enabledProperties[ii]);
            }
        }

        private void PieceGUI(T piece, int listIndex)
        {
            var propertyDrawer = _enabledProperties[listIndex];
            propertyDrawer.SetFieldName(listIndex.ToString());
            propertyDrawer.ShowFieldName = ShowFieldName;
            
            // ((PropertyDrawer<T>) propertyDrawer).SetValue(piece);
            propertyDrawer.GetType().GetMethod("SetValue").Invoke(propertyDrawer, new []
            {
                piece as object
            });

            propertyDrawer.OnValueChanged += () =>
            {
                var value = GetValue();
                // value[listIndex] = ((PropertyDrawer<T>) propertyDrawer).GetValue();
                value[listIndex] = (T) propertyDrawer.GetType().GetMethod("GetValue").Invoke(propertyDrawer, null);
                
                SetValue(value);
                ItemValueChanged?.Invoke(listIndex, value[listIndex]);
            };

            EditorGUILayout.BeginHorizontal();
            
            propertyDrawer.OnGUI();

            if(CanDeleteItem)
            {
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
            }
            
            EditorGUILayout.EndHorizontal();
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

            availablePropertyDrawer.ShowFieldName = ShowFieldName;
            _enabledProperties.Add(availablePropertyDrawer);

            return availablePropertyDrawer;
        }
    }
}
#endif