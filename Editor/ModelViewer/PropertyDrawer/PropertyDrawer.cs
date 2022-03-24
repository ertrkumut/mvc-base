using System.Reflection;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    internal class PropertyDrawer<TPropertyType> : PropertyDrawerBase
    {
        public TPropertyType PropertyType { get; set; }
        
        public PropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
        {
        }

        public TPropertyType GetPropertyValue()
        {
            return (TPropertyType) _fieldInfo.GetValue(_targetObject);
        }

        public void SetValue(TPropertyType newValue)
        {
            _fieldInfo.SetValue(_targetObject, newValue);
        }
    }
}