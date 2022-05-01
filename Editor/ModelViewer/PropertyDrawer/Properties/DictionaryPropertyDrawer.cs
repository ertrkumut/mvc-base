using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class DictionaryPropertyDrawer<TKey, TValue> : PropertyDrawer<Dictionary<TKey, TValue>>
    {
        private bool _foldOut;
        
        private Dictionary<Type, List<PropertyDrawerBase>> _enabledProperties;
        private Dictionary<Type, List<PropertyDrawerBase>> _disabledProperties;

        private Type _keyType;
        private Type _valueType;

        public DictionaryPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
            _enabledProperties = new Dictionary<Type, List<PropertyDrawerBase>>();
            _disabledProperties = new Dictionary<Type, List<PropertyDrawerBase>>();

            _keyType = typeof(TKey);
            _valueType = typeof(TValue);
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
                    SetValue(new Dictionary<TKey, TValue>());

                EditorGUILayout.EndVertical();
                return;
            }
            
            SendPropertyDrawersToPool();
            DisplayDictionary();
            
            EditorGUILayout.EndVertical();
        }

        private void DisplayDictionary()
        {
            var dictValue = GetValue();
            var keys = dictValue.Keys.ToList();
            var values = dictValue.Values.ToList();
            
            for (var ii = 0; ii < keys.Count; ii++)
            {
                var key = keys[ii];
                var value = values[ii];

                EditorGUILayout.BeginHorizontal();
                
                var keyPropertyDrawer = GetAvailablePropertyDrawer(_keyType);
                keyPropertyDrawer.SetFieldName("");
                ((PropertyDrawer<TKey>) keyPropertyDrawer).SetValue(key);
                keyPropertyDrawer.OnValueChanged += () =>
                {
                    //TODO; Update dictionary keys
                };
                
                var valuePropertyDrawer = GetAvailablePropertyDrawer(_valueType);
                valuePropertyDrawer.SetFieldName("");
                ((PropertyDrawer<TValue>) valuePropertyDrawer).SetValue(value);
                valuePropertyDrawer.OnValueChanged += () =>
                {
                    dictValue[key] = ((PropertyDrawer<TValue>) valuePropertyDrawer).GetValue();
                };
                
                keyPropertyDrawer.OnGUI();
                valuePropertyDrawer.OnGUI();
                
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void SendPropertyDrawersToPool()
        {
            for (var ii = 0; ii < _enabledProperties.Count; ii++)
            {
                var propertyType = _enabledProperties.Keys.ElementAt(0);
                var propertyList = _enabledProperties[propertyType];
                for (var propertyDrawerIndex = 0; propertyDrawerIndex < propertyList.Count; propertyDrawerIndex++)
                {
                    var property = propertyList[0];
                    SendPropertyDrawerToPool(property, propertyType);
                }
            }
        }
        
        private void SendPropertyDrawerToPool(PropertyDrawerBase propertyDrawerBase, Type propertyType)
        {
            propertyDrawerBase.OnValueChanged = null;

            _enabledProperties[propertyType].Remove(propertyDrawerBase);
            _disabledProperties[propertyType].Add(propertyDrawerBase);
        }

        private PropertyDrawerBase GetAvailablePropertyDrawer(Type propertyType)
        {
            PropertyDrawerBase availablePropertyDrawer = null;
            
            if(!_disabledProperties.ContainsKey(propertyType))
                _disabledProperties.Add(propertyType, new List<PropertyDrawerBase>());

            var poolList = _disabledProperties[propertyType];

            if (poolList.Count == 0)
            {
                var propertyDrawerType = ModelViewerUtils.GetPropertyDrawerType(propertyType);
                availablePropertyDrawer = (PropertyDrawerBase) Activator.CreateInstance(propertyDrawerType, _fieldName, _readOnly);
            }
            else
            {
                availablePropertyDrawer = poolList[0];
                poolList.Remove(availablePropertyDrawer);
            }

            if(!_enabledProperties.ContainsKey(propertyType))
                _enabledProperties.Add(propertyType, new List<PropertyDrawerBase>());
            
            _enabledProperties[propertyType].Add(availablePropertyDrawer);
            
            return availablePropertyDrawer;
        }
    }
}