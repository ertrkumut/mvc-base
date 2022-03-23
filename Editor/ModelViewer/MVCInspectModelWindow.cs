using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;
using MVC.Runtime.Attributes;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer
{
    public class MVCInspectModelWindow : EditorWindow
    {
        private object _inspectedObject;
        private object _inspectedObjectContext;

        // Property Type -- PropertyDrawer Type
        private Dictionary<Type, Type> _propertyDrawerTypesDict;
        private Dictionary<FieldInfo, MVCPropertyDrawerBase> _activePropertyDrawersDict;
        
        public void Initialize(object inspectedObject, object inspectedObjectContext)
        {
            _inspectedObject = inspectedObject;
            _inspectedObjectContext = inspectedObjectContext;
            
            InitializePropertyDrawerTypes();
        }

        private void InitializePropertyDrawerTypes()
        {
            _activePropertyDrawersDict = new Dictionary<FieldInfo, MVCPropertyDrawerBase>();
            _propertyDrawerTypesDict = new Dictionary<Type, Type>();
            
            var propertyDrawerTypes = GetType().Assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(MVCPropertyDrawerBase)))
                .ToList();

            foreach (var propertyDrawerType in propertyDrawerTypes)
            {
                var drawerType = propertyDrawerType.GetProperty("PropertyType").PropertyType;
                _propertyDrawerTypesDict.Add(drawerType, propertyDrawerType);
            }
        }

        private void OnGUI()
        {
            if(_inspectedObject == null)
                return;
            
            DisplayObjectFields(_inspectedObject);
        }

        private void DisplayObjectFields(object rootObject)
        {
            var rootType = rootObject.GetType();
            var fieldInfoList = rootType.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var fieldInfo in fieldInfoList)
            {
                var fieldAttributes = fieldInfo.GetCustomAttributes(typeof(HideFromModelViewerAttribute)).ToList();
                if(fieldAttributes.Count != 0)
                    continue;
                
                DisplayFieldInfoGUI(fieldInfo, rootObject);
            }
        }

        private void DisplayFieldInfoGUI(FieldInfo fieldInfo, object rootObject)
        {
            var fieldType = fieldInfo.FieldType;
            
            if (fieldType.IsClass && !IsPropertyTypeExist(fieldType) && !fieldType.IsSubclassOf(typeof(Object)))
            {
                EditorGUILayout.BeginVertical("box");
                
                EditorGUILayout.LabelField(fieldInfo.Name);
                var fieldValue = fieldInfo.GetValue(rootObject);
                DisplayObjectFields(fieldValue);
                
                EditorGUILayout.EndVertical();
                return;
            }
            
            var propertyDrawer = GetPropertyDrawer(fieldInfo, rootObject);
            propertyDrawer?.OnGUI();
        }

        private MVCPropertyDrawerBase GetPropertyDrawer(FieldInfo fieldInfo, object rootObject)
        {
            MVCPropertyDrawerBase propertyDrawer = null;
            if (!_activePropertyDrawersDict.ContainsKey(fieldInfo))
            {
                var targetPropertyType = fieldInfo.FieldType;
                if (targetPropertyType.IsSubclassOf(typeof(Object)))
                    targetPropertyType = typeof(Object);

                if (!_propertyDrawerTypesDict.ContainsKey(targetPropertyType))
                    return null;
                
                var propertyDrawerType = _propertyDrawerTypesDict[targetPropertyType];
                propertyDrawer = (MVCPropertyDrawerBase) Activator.CreateInstance(propertyDrawerType, fieldInfo, rootObject);
                _activePropertyDrawersDict.Add(fieldInfo, propertyDrawer);
            }
            else
                propertyDrawer = _activePropertyDrawersDict[fieldInfo];
            
            return propertyDrawer;
        }

        public bool IsPropertyTypeExist(Type type)
        {
            return _propertyDrawerTypesDict.ContainsKey(type);
        }
        
        private void OnDestroy()
        {
            _propertyDrawerTypesDict = new Dictionary<Type, Type>();
            _activePropertyDrawersDict = new Dictionary<FieldInfo, MVCPropertyDrawerBase>();
        }
    }
}