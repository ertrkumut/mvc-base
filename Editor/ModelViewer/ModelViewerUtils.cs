using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;
using MVC.Runtime.Attributes;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer
{
    internal static class ModelViewerUtils
    {
        private static Dictionary<Type, Type> _propertyDrawerTypesDict;

        public static List<MemberInfo> GetTypeMembersList(object rootObject)
        {
            var rootType = rootObject.GetType();
            
            var fieldInfoList = rootType
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(fieldInfo => fieldInfo.GetCustomAttributes(typeof(HideInModelViewerAttribute)).ToList().Count == 0)
                .Cast<MemberInfo>()
                .ToList();
            
            var propertyInfoList = rootType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(propertyInfo => propertyInfo.GetCustomAttributes(typeof(HideInModelViewerAttribute)).ToList().Count == 0)
                .Cast<MemberInfo>()
                .ToList();

            return fieldInfoList.Concat(propertyInfoList).ToList();
        }
        
        public static bool IsPropertyDrawerTypeExist(Type type)
        {
            if(_propertyDrawerTypesDict == null)
                InitializePropertyDrawerTypes();
            
            var result = _propertyDrawerTypesDict.ContainsKey(type);
            if (result)
                return true;

            if (typeof(IList).IsAssignableFrom(type))
                return true;
            
            return false;
        }
        
        public static Type GetPropertyDrawerType(Type propertyType)
        {
            if(_propertyDrawerTypesDict == null)
                InitializePropertyDrawerTypes();

            if (propertyType.IsSubclassOf(typeof(Object)))
                propertyType = typeof(Object);

            Type propertyDrawerType;
                
            if (!_propertyDrawerTypesDict.ContainsKey(propertyType))
            {
                if (typeof(IList).IsAssignableFrom(propertyType))
                {
                    var listPropertyDrawer = typeof(MemoryInfoDrawerBase)
                        .Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.Name.Contains("ListPropertyDrawer"))
                        .MakeGenericType(propertyType.GetGenericArguments());

                    propertyDrawerType = listPropertyDrawer;
                }
                else
                    return null;
            }
            else
                propertyDrawerType = _propertyDrawerTypesDict[propertyType];
            
            return propertyDrawerType;
        }
        
        private static void InitializePropertyDrawerTypes()
        {
            _propertyDrawerTypesDict = new Dictionary<Type, Type>();
            
            var propertyDrawerTypes = typeof(MemoryInfoDrawerBase).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(MemoryInfoDrawerBase)))
                .ToList();

            foreach (var propertyDrawerType in propertyDrawerTypes)
            {
                var drawerType = propertyDrawerType.GetProperty("PropertyType").PropertyType;
                _propertyDrawerTypesDict.Add(drawerType, propertyDrawerType);
            }
        }

        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
                return (memberInfo as FieldInfo).FieldType;
            else if(memberInfo.MemberType == MemberTypes.Property)
                return (memberInfo as PropertyInfo).PropertyType;

            return null;
        }

        public static object GetValue(this MemberInfo memberInfo, object rootObject)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
                return (memberInfo as FieldInfo).GetValue(rootObject);
            else if(memberInfo.MemberType == MemberTypes.Property)
                return (memberInfo as PropertyInfo).GetValue(rootObject);

            return null;
        }

        public static void SetValue(this MemberInfo memberInfo, object rootObject, object newValue)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
                (memberInfo as FieldInfo).SetValue(rootObject, newValue);
            else if(memberInfo.MemberType == MemberTypes.Property)
                (memberInfo as PropertyInfo).SetValue(rootObject, newValue);
        }
    }
}