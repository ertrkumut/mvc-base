using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;

namespace MVC.Editor.ModelViewer
{
    internal static class ModelViewerUtils
    {
        private static Dictionary<Type, Type> _propertyDrawerTypesDict;

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
        
        public static Type GetPropertyDrawer(MemberInfo memberInfo, object rootObject)
        {
            if(_propertyDrawerTypesDict == null)
                InitializePropertyDrawerTypes();
            
            Type propertyType = null;

            if (memberInfo.MemberType == MemberTypes.Field)
                propertyType = (memberInfo as FieldInfo).FieldType;
            else if(memberInfo.MemberType == MemberTypes.Property)
                propertyType = (memberInfo as PropertyInfo).PropertyType;
            
            if (propertyType.IsSubclassOf(typeof(Object)))
                propertyType = typeof(Object);

            Type propertyDrawerType;
                
            if (!_propertyDrawerTypesDict.ContainsKey(propertyType))
            {
                if (typeof(IList).IsAssignableFrom(propertyType))
                {
                    var listPropertyDrawer = typeof(PropertyDrawerBase).Assembly
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
            
            var propertyDrawerTypes = typeof(PropertyDrawerBase).Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(PropertyDrawerBase)))
                .ToList();

            foreach (var propertyDrawerType in propertyDrawerTypes)
            {
                var drawerType = propertyDrawerType.GetProperty("PropertyType").PropertyType;
                _propertyDrawerTypesDict.Add(drawerType, propertyDrawerType);
            }
        }
    }
}