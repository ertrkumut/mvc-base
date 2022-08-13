#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.MemberInfoDrawer;
using MVC.Editor.ModelViewer.PropertyDrawer;
using MVC.Runtime.Attributes;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer
{
    internal static class ModelViewerUtils
    {
        private static Dictionary<Type, Type> _memberInfoDrawerTypesDict;
        private static Dictionary<Type, Type> _propertyDrawerTypesDict;

        public static List<MemberInfo> GetTypeMembersList(object rootObject)
        {
            var rootType = rootObject.GetType();
            
            var publicFieldInfoList = rootType
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(fieldInfo => 
                    fieldInfo.GetCustomAttributes(typeof(HideInModelViewerAttribute)).ToList().Count == 0)
                .Cast<MemberInfo>()
                .ToList();

            var privateFieldInfoList = rootType
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(fieldInfo =>
                    fieldInfo.GetCustomAttributes(typeof(ShowInModelViewerAttribute)).ToList().Count != 0)
                .Cast<MemberInfo>()
                .ToList();
            
            var publicPropertyInfoList = rootType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(propertyInfo => 
                    propertyInfo.GetCustomAttributes(typeof(HideInModelViewerAttribute)).ToList().Count == 0)
                .Cast<MemberInfo>()
                .ToList();

            var privatePropertyInfoList = rootType
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(propertyInfo =>
                    propertyInfo.GetCustomAttributes(typeof(ShowInModelViewerAttribute)).ToList().Count != 0)
                .Cast<MemberInfo>()
                .ToList();

            var allPropertyInfoList = publicPropertyInfoList.Concat(privatePropertyInfoList).ToList();
            var allFieldInfoList = publicFieldInfoList.Concat(privateFieldInfoList).ToList();
            
            return allFieldInfoList.Concat(allPropertyInfoList).ToList();
        }
        
        public static bool IsPropertyDrawerTypeExist(Type type)
        {
            if(_memberInfoDrawerTypesDict == null)
                InitializeMemberInfoDrawerTypes();
            
            var result = _memberInfoDrawerTypesDict.ContainsKey(type);
            if (result)
                return true;

            if (typeof(IList).IsAssignableFrom(type))
                return true;

            if (typeof(IDictionary).IsAssignableFrom(type))
                return true;
            
            return false;
        }
        
        public static Type GetMemberInfoDrawerType(Type memberInfoType)
        {
            if(_memberInfoDrawerTypesDict == null)
                InitializeMemberInfoDrawerTypes();

            if (memberInfoType.IsSubclassOf(typeof(Object)))
                memberInfoType = typeof(Object);

            Type memberInfoDrawerType;
                
            if (!_memberInfoDrawerTypesDict.ContainsKey(memberInfoType))
            {
                if (typeof(IList).IsAssignableFrom(memberInfoType))
                {
                    var listMemberInfoDrawer = typeof(MemberInfoDrawerBase)
                        .Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.Name.Contains("ListMemberInfoDrawer"))
                        .MakeGenericType(memberInfoType.GetGenericArguments());

                    memberInfoDrawerType = listMemberInfoDrawer;
                }
                else if (typeof(IDictionary).IsAssignableFrom(memberInfoType))
                {
                    var dictMemberInfoDrawer = typeof(MemberInfoDrawerBase)
                        .Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.Name.Contains("DictMemberInfoDrawer"))
                        .MakeGenericType(memberInfoType.GetGenericArguments());

                    memberInfoDrawerType = dictMemberInfoDrawer;
                }
                else
                    return null;
            }
            else
                memberInfoDrawerType = _memberInfoDrawerTypesDict[memberInfoType];
            
            return memberInfoDrawerType;
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
                    var listPropertyDrawer = typeof(MemberInfoDrawerBase)
                        .Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.Name.Contains("ListPropertyDrawer"))
                        .MakeGenericType(propertyType.GetGenericArguments());

                    propertyDrawerType = listPropertyDrawer;
                }
                else if (propertyType.IsClass)
                {
                    var classPropertyDrawer = typeof(MemberInfoDrawerBase)
                        .Assembly
                        .GetTypes()
                        .FirstOrDefault(x => x.Name.Contains("ClassPropertyDrawer"))
                        .MakeGenericType(propertyType);
                    return classPropertyDrawer;
                }
                else    
                    return null;
            }
            else
                propertyDrawerType = _propertyDrawerTypesDict[propertyType];
            
            return propertyDrawerType;
        }
        
        private static void InitializeMemberInfoDrawerTypes()
        {
            _memberInfoDrawerTypesDict = new Dictionary<Type, Type>();
            
            var propertyDrawerTypes = typeof(MemberInfoDrawerBase).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(MemberInfoDrawerBase)))
                .ToList();

            foreach (var propertyDrawerType in propertyDrawerTypes)
            {
                var drawerType = propertyDrawerType.GetProperty("PropertyType").PropertyType;
                _memberInfoDrawerTypesDict.Add(drawerType, propertyDrawerType);
            }
        }

        private static void InitializePropertyDrawerTypes()
        {
            _propertyDrawerTypesDict = new Dictionary<Type, Type>();
            
            var propertyDrawerTypes = typeof(PropertyDrawerBase)
                .Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(PropertyDrawerBase)))
                .ToList();

            foreach (var propertyDrawerType in propertyDrawerTypes)
            {
                var drawerType = propertyDrawerType.GetProperty("PropertyType")?.PropertyType;
                if(drawerType == null)
                    continue;
                
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
#endif