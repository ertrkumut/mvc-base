#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class DictionaryPropertyDrawer<TKey, TValue> : PropertyDrawer<Dictionary<TKey, TValue>>
    {
        private bool _foldOut;

        private ListPropertyDrawer<TKey> _keyDrawer;
        private ListPropertyDrawer<TValue> _valueDrawer;

        public DictionaryPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
            _keyDrawer = new ListPropertyDrawer<TKey>("Key", true);
            _valueDrawer = new ListPropertyDrawer<TValue>("Value", readOnly);

            _keyDrawer.UseFoldOut = false;
            _valueDrawer.UseFoldOut = false;

            _keyDrawer.ShowFieldName = false;
            _valueDrawer.ShowFieldName = false;

            _keyDrawer.CanDeleteItem = false;
            _valueDrawer.CanDeleteItem = false;
            
            _keyDrawer.ItemValueChanged += KeyChanged;
            _keyDrawer.ItemValueChanged -= KeyChanged;
        }

        protected override void OnBeforeDrawGUI()
        {
            base.OnBeforeDrawGUI();

            var dictValue = GetValue();
            _keyDrawer.SetValue(dictValue.Keys.ToList());
            _valueDrawer.SetValue(dictValue.Values.ToList());
            
            EditorGUILayout.BeginVertical("box");
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();

            _foldOut = EditorGUILayout.Foldout(_foldOut, _fieldName);

            if (!_foldOut)
                return;
            
            var value = GetValue();
            if (value == null)
            {
                if (GUILayout.Button("NULL")) 
                    SetValue(new Dictionary<TKey, TValue>());
                
                return;
            }

            EditorGUILayout.BeginHorizontal();
            
            _keyDrawer.OnGUI();
            _valueDrawer.OnGUI();
            
            EditorGUILayout.EndHorizontal();
        }

        protected override void OnDrawCompletedGUI()
        {
            base.OnDrawCompletedGUI();
            EditorGUILayout.EndVertical();
        }
        
        private void KeyChanged(int itemIndex, TKey newValue)
        {
            
        }
        
        private void ValueChanged(int itemIndex, TKey newValue)
        {
            
        }
    }
}
#endif