using System;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer
{
    internal class MemberInfoDrawer<TPropertyType> : MemberInfoDrawerBase
    {
        public TPropertyType PropertyType { get; set; }
        
        public MemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }

        protected override void CreatePropertyDrawer()
        {
            base.CreatePropertyDrawer();

            _propertyDrawer = (PropertyDrawerBase) Activator.CreateInstance(_propertyDrawerType, _fieldName, _hasPropertyReadOnly);

            ((PropertyDrawer<TPropertyType>) _propertyDrawer).SetValue(GetPropertyValue());
            _propertyDrawer.OnValueChanged += () =>
            {
                SetValue(((PropertyDrawer<TPropertyType>) _propertyDrawer).GetValue());
            };
        }

        public TPropertyType GetPropertyValue()
        {
            return (TPropertyType) _memberInfo.GetValue(_targetObject);
        }

        public void SetValue(TPropertyType newValue)
        {
            _memberInfo.SetValue(_targetObject, newValue);
        }
    }
}