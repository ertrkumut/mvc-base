using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.PropertyDrawer;
using MVC.Runtime.Attributes;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer
{
    internal class InspectWindow : EditorWindow
    {
        private object _inspectedObject;
        private object _inspectedObjectContext;
        
        private Dictionary<MemberInfo, PropertyDrawerBase> _activePropertyDrawersDict;
        
        public void Initialize(object inspectedObject, object inspectedObjectContext)
        {
            _inspectedObject = inspectedObject;
            _inspectedObjectContext = inspectedObjectContext;
            
            InitializePropertyDrawerTypes();
        }

        private void InitializePropertyDrawerTypes()
        {
            _activePropertyDrawersDict = new Dictionary<MemberInfo, PropertyDrawerBase>();
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
                var fieldAttributes = fieldInfo.GetCustomAttributes(typeof(HideInModelViewerAttribute)).ToList();
                if(fieldAttributes.Count != 0)
                    continue;
                
                DisplayFieldInfoGUI(fieldInfo, rootObject);
            }
        }

        private void DisplayFieldInfoGUI(FieldInfo fieldInfo, object rootObject)
        {
            var fieldType = fieldInfo.FieldType;
            
            if (fieldType.IsClass && !ModelViewerUtils.IsPropertyDrawerTypeExist(fieldType) && !fieldType.IsSubclassOf(typeof(Object)))
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

        private PropertyDrawerBase GetPropertyDrawer(FieldInfo fieldInfo, object rootObject)
        {
            PropertyDrawerBase propertyDrawer = null;
            if (!_activePropertyDrawersDict.ContainsKey(fieldInfo))
            {
                Type propertyDrawerType = ModelViewerUtils.GetPropertyDrawer(fieldInfo, rootObject);

                propertyDrawer = (PropertyDrawerBase) Activator.CreateInstance(propertyDrawerType, fieldInfo, rootObject);
                _activePropertyDrawersDict.Add(fieldInfo, propertyDrawer);
            }
            else
                propertyDrawer = _activePropertyDrawersDict[fieldInfo];
            
            return propertyDrawer;
        }

        private void OnDestroy()
        {
            _activePropertyDrawersDict = new Dictionary<MemberInfo, PropertyDrawerBase>();
        }
    }
}