using System.Linq;
using System.Reflection;
using MVC.Runtime.Attributes;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    internal class PropertyDrawerBase
    {
        protected MemberInfo _memberInfo;
        protected object _targetObject;

        protected string _fieldName;

        private bool _hasPropertyReadOnly;
        
        public PropertyDrawerBase(MemberInfo memberInfo, object targetObject)
        {
            _memberInfo = memberInfo;
            _targetObject = targetObject;

            if(_memberInfo == null)
                return;
            
            _fieldName = memberInfo.Name.Replace("<", "").Replace(">k__backingField", "");

            _hasPropertyReadOnly = memberInfo.GetCustomAttributes(typeof(ReadOnlyAttribute)).ToList().Count != 0;
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