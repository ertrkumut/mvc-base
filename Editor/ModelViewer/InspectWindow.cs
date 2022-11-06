#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.ModelViewer.MemberInfoDrawer;
using MVC.Runtime.Root;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer
{
    internal class InspectWindow : EditorWindow
    {
        private object _inspectedObject;
        private object _inspectedObjectContext;
        
        private Dictionary<MemberInfo, MemberInfoDrawerBase> _activePropertyDrawersDict;

        public void Initialize(object inspectedObject, object inspectedObjectContext, string bindingName = "")
        {
            _inspectedObject = inspectedObject;
            _inspectedObjectContext = inspectedObjectContext;
            
            _activePropertyDrawersDict = new Dictionary<MemberInfo, MemberInfoDrawerBase>();

            if (inspectedObjectContext != null)
            {
                PlayerPrefs.SetString(titleContent.text, inspectedObjectContext.GetType().Name + "_" + inspectedObject.GetType().Name + "_" + bindingName);
            }
        }

        public void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("Enter Play Mode to view Models.");
                return;
            }
            
            if(_inspectedObject == null && _inspectedObjectContext == null)
            {
                FindContextAndLoadObjectReferenceFromContext();
                return;
            }
            else if(_inspectedObject == null)
                return;
            
            DisplayObjectFields(_inspectedObject);
        }

        private void Update()
        {
            if(!Application.isPlaying)
                return;

            if(_inspectedObject == null)
                return;
            
            Repaint();
        }

        private void DisplayObjectFields(object rootObject)
        {
            var memberInfoList = ModelViewerUtils.GetTypeMembersList(rootObject);

            foreach (var memberInfo in memberInfoList)
            {
                DisplayFieldInfoGUI(memberInfo, rootObject);
            }
        }

        private void DisplayFieldInfoGUI(MemberInfo memberInfo, object rootObject)
        {
            var memberType = memberInfo.GetMemberType();

            if (memberType.IsInterface)
            {
                if(memberInfo.GetValue(rootObject) != null)
                    memberType = memberInfo.GetValue(rootObject).GetType();
                else
                    return;
            }
            
            if (memberType.IsClass && !ModelViewerUtils.IsPropertyDrawerTypeExist(memberType) && !memberType.IsSubclassOf(typeof(Object)))
            {
                EditorGUILayout.BeginVertical("box");
                
                EditorGUILayout.LabelField(memberInfo.Name);
                var fieldValue = memberInfo.GetValue(rootObject);
                DisplayObjectFields(fieldValue);
                
                EditorGUILayout.EndVertical();
                return;
            }
            
            var propertyDrawer = GetPropertyDrawer(memberInfo, rootObject);
            propertyDrawer?.OnGUI();
        }

        private MemberInfoDrawerBase GetPropertyDrawer(MemberInfo memberInfo, object rootObject)
        {
            MemberInfoDrawerBase memberInfoDrawer = null;
            if (!_activePropertyDrawersDict.ContainsKey(memberInfo))
            {
                var memberType = memberInfo.GetMemberType();

                if (memberType.IsInterface)
                {
                    if(memberInfo.GetValue(rootObject) != null)
                        memberType = memberInfo.GetValue(rootObject).GetType();
                    else
                        return null;
                }
                
                Type propertyDrawerType = ModelViewerUtils.GetMemberInfoDrawerType(memberType);

                memberInfoDrawer = (MemberInfoDrawerBase) Activator.CreateInstance(propertyDrawerType, memberInfo, rootObject);
                _activePropertyDrawersDict.Add(memberInfo, memberInfoDrawer);
            }
            else
                memberInfoDrawer = _activePropertyDrawersDict[memberInfo];
            
            return memberInfoDrawer;
        }

        private void FindContextAndLoadObjectReferenceFromContext()
        {
            // inspectedObjectContext.GetType().Name + "_" + bindingName
            var titleString = PlayerPrefs.GetString(titleContent.text);
            var contextName = titleString.Split('_')[0];
            var objectTypeName = titleString.Split('_')[1];
            var bindingName = titleString.Split('_')[2];

            var context = FindObjectsOfType<RootBase>()
                .Select(x => x.GetContext())
                .ToList()
                .FirstOrDefault(x => x.GetType().Name == contextName);

            var binding = context.InjectionBinder
                .GetInjectedInstances()
                .FirstOrDefault(x => x.Value.GetType().Name == objectTypeName && x.Name == bindingName);
            
            if(binding == null)
                binding = context.InjectionBinderCrossContext
                    .GetInjectedInstances()
                    .FirstOrDefault(x => x.Value.GetType().Name == objectTypeName && x.Name == bindingName);

            if (binding != null)
                _inspectedObject = binding.Value;
            
            _activePropertyDrawersDict = new Dictionary<MemberInfo, MemberInfoDrawerBase>();
        }
        
        private void OnDestroy()
        {
            _activePropertyDrawersDict = new Dictionary<MemberInfo, MemberInfoDrawerBase>();
        }
    }
}
#endif