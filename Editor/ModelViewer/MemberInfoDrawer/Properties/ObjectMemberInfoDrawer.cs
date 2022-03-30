using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class ObjectMemberInfoDrawer : MemberInfoDrawer<Object>
    {
        public ObjectMemberInfoDrawer(FieldInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
        
        public override void OnDrawGUI()
        {
            base.OnDrawGUI();

            Type propertyType = null;

            if (_memberInfo.MemberType == MemberTypes.Field)
                propertyType = (_memberInfo as FieldInfo).FieldType;
            else if(_memberInfo.MemberType == MemberTypes.Property)
                propertyType = (_memberInfo as PropertyInfo).PropertyType;
            
            var propertyValue = GetPropertyValue();
            var newValue = EditorGUILayout.ObjectField(new GUIContent(_fieldName), propertyValue, propertyType, true);
            if(newValue != propertyValue)
                SetValue(newValue);
        }
    }
}