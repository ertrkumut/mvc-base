using System.Linq;
using System.Reflection;
using MVC.Runtime.Attributes;
using UnityEngine;

namespace MVC.Runtime.Editor.ModelViewer.PropertyDrawer
{
    public class MVCPropertyDrawerBase
    {
        protected FieldInfo _fieldInfo;
        protected object _targetObject;

        protected string _fieldName;

        private bool _hasPropertyReadOnly;
        
        public MVCPropertyDrawerBase(FieldInfo fieldInfo, object targetObject)
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

        protected void OnBeforeDrawGUI()
        {
            GUI.enabled = !_hasPropertyReadOnly;
        }
        
        public virtual void OnDrawGUI()
        {
        }
        
        protected void OnDrawCompletedGUI(){}
    }
}