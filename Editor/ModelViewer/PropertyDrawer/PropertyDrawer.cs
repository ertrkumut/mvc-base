using System;

namespace MVC.Editor.ModelViewer.PropertyDrawer
{
    public class PropertyDrawer<TPropertyType> : PropertyDrawerBase, IDisposable
    {
        public Action<TPropertyType> OnValueChanged;
        
        protected TPropertyType _property;
        
        public PropertyDrawer(TPropertyType property, string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
            _property = property;
        }

        public TPropertyType GetValue()
        {
            return _property;
        }

        public void SetValue(TPropertyType newValue)
        {
            _property = newValue;
            OnValueChanged?.Invoke(_property);
        }

        public void Dispose()
        {
            OnValueChanged = null;
        }
    }
}