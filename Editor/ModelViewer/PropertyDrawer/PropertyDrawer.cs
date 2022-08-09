#if UNITY_EDITOR
using System;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    internal class PropertyDrawer<TPropertyType> : PropertyDrawerBase, IDisposable
    {
        public TPropertyType PropertyType { get; set; }
        
        protected TPropertyType _property;
        
        public PropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }

        public TPropertyType GetValue()
        {
            return _property;
        }

        public void SetValue(TPropertyType newValue)
        {
            _property = newValue;
            OnValueChanged?.Invoke();
        }

        public void Dispose()
        {
        }
    }
}
#endif