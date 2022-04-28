using System;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;
using MVC.Runtime.Attributes;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer
{
    internal class MemberInfoDrawerBase
    {
        protected virtual Type _propertyDrawerType { get; }
        protected PropertyDrawerBase _propertyDrawer;

        protected MemberInfo _memberInfo;
        protected object _targetObject;

        protected string _fieldName;

        protected bool _hasPropertyReadOnly;
        
        public MemberInfoDrawerBase(MemberInfo memberInfo, object targetObject)
        {
            _memberInfo = memberInfo;
            _targetObject = targetObject;

            if(_memberInfo == null)
                return;
            
            _fieldName = memberInfo.Name.Replace("<", "").Replace(">k__backingField", "");

            _hasPropertyReadOnly = memberInfo.GetCustomAttributes(typeof(ReadOnlyAttribute)).ToList().Count != 0;

            if (_propertyDrawerType != null)
            {
                CreatePropertyDrawer();
            }
        }
        
        protected virtual void CreatePropertyDrawer(){}
        
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
            if(_propertyDrawer == null)
                return;
            
            _propertyDrawer.OnGUI();
        }

        protected virtual void OnDrawCompletedGUI()
        {
            GUI.enabled = true;
        }
    }
}