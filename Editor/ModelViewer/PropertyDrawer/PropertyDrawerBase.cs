#if UNITY_EDITOR
using System;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    internal class PropertyDrawerBase
    {
        public Action OnValueChanged;
        
        protected bool _readOnly;
        protected string _fieldName;

        public bool ShowFieldName = true;
        
        public PropertyDrawerBase(string fieldName, bool readOnly)
        {
            _readOnly = readOnly;
            _fieldName = fieldName;
        }

        public void SetFieldName(string fieldName)
        {
            _fieldName = fieldName;
        }
        
        public void OnGUI()
        {
            OnBeforeDrawGUI();
            OnDrawGUI();
            OnDrawCompletedGUI();
        }

        protected virtual void OnBeforeDrawGUI()
        {
            GUI.enabled = !_readOnly;
        }
        
        protected virtual void OnDrawGUI()
        {
        }
        
        protected virtual void OnDrawCompletedGUI(){}
    }
}
#endif