using System.Reflection;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    internal class PropertyDrawer<TPropertyType> : PropertyDrawerBase
    {
        public TPropertyType PropertyType { get; set; }
        
        public PropertyDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
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