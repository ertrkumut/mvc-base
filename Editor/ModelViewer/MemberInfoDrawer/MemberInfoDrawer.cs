using System.Reflection;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer
{
    internal class MemberInfoDrawer<TPropertyType> : MemberInfoDrawerBase
    {
        public TPropertyType PropertyType { get; set; }
        
        public MemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
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