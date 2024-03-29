﻿#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;
using MVC.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer
{
    internal class MemberInfoDrawerBase
    {
        protected virtual Type _propertyDrawerType { get; set; }
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

            CreatePropertyDrawer();
        }
        
        protected virtual void CreatePropertyDrawer(){}
        
        public void OnGUI()
        {
            OnBeforeDrawGUI();
            
            if(_propertyDrawerType != null)
                OnDrawGUI();
            else
                EditorGUILayout.HelpBox(new GUIContent($"{_fieldName}: Type is not supported!"));
            
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
#endif