using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class ListMemberInfoDrawer<T> : MemberInfoDrawer<List<T>>
    {
        private bool _foldOut;

        private List<PropertyDrawerBase> _enabledProperties;
        private List<PropertyDrawerBase> _disabledProperties;

        public ListMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
            _enabledProperties = new List<PropertyDrawerBase>();
            _disabledProperties = new List<PropertyDrawerBase>();
        }

        protected override void CreatePropertyDrawer()
        {
            if (typeof(IList).IsAssignableFrom(typeof(T)))
            {
                //TODO: Recursive List Drawer
            }
            else
            {
                _propertyDrawerType = ModelViewerUtils.GetPropertyDrawerType(typeof(T));
            }
        }

        public override void OnDrawGUI()
        {
            EditorGUILayout.BeginVertical("box");

            _foldOut = EditorGUILayout.Foldout(_foldOut, _memberInfo.Name);

            if (!_foldOut)
            {
                EditorGUILayout.EndVertical();
                return;
            }
            
            var value = GetPropertyValue();
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
                var value = GetPropertyValue();
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
                
                var listValue = GetPropertyValue();
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
            PropertyDrawerBase availablePropertyDrawer = null;
            if (_disabledProperties.Count == 0)
            {
                availablePropertyDrawer =
                    (PropertyDrawerBase) Activator.CreateInstance(_propertyDrawerType, _fieldName,
                        _hasPropertyReadOnly);
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