using System.Linq;
using System.Reflection;
using MVC.Runtime.Attributes;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    internal class PropertyDrawerBase
    {
        protected FieldInfo _fieldInfo;
        protected object _targetObject;

        protected string _fieldName;

        private bool _hasPropertyReadOnly;
        
        public PropertyDrawerBase(FieldInfo fieldInfo, object targetObject)
        {
            _fieldInfo = fieldInfo;
            _targetObject = targetObject;

            _fieldName = fieldInfo.Name.Replace("<", "").Replace(">k__backingField", "");

            _hasPropertyReadOnly = fieldInfo.GetCustomAttributes(typeof(ReadOnlyAttribute)).ToList().Count != 0;
        }
        
        public void OnGUI()
        {
            OnBeforeDrawGUI();
            OnDrawGUI();
            OnDrawCompletedGUI();
        }

        protected virtual void OnBeforeDrawGUI()
        {
            GUI.enabled = !_hasPropertyReadOnly;
        }
        
        public virtual void OnDrawGUI()
        {
        }
        
        protected virtual void OnDrawCompletedGUI(){}
    }
}