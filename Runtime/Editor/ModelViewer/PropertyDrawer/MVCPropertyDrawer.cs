using System.Reflection;

namespace MVC.Runtime.Editor.ModelViewer.PropertyDrawer
{
    public class MVCPropertyDrawer<TPropertyType> : MVCPropertyDrawerBase
    {
        public TPropertyType PropertyType { get; set; }
        
        public MVCPropertyDrawer(FieldInfo fieldInfo, object targetObject) : base(fieldInfo, targetObject)
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