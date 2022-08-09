#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer.Properties;

namespace MVC.Editor.ModelViewer.MemberInfoDrawer.Properties
{
    internal class DictMemberInfoDrawer<TKey, TValue> : MemberInfoDrawer<Dictionary<TKey, TValue>>
    {
        protected override Type _propertyDrawerType => typeof(DictionaryPropertyDrawer<TKey, TValue>);

        public DictMemberInfoDrawer(MemberInfo memberInfo, object targetObject) : base(memberInfo, targetObject)
        {
        }
    }
}
#endif