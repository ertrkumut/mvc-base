using System;
using System.Collections.Generic;
using System.Reflection;
using MVC.Editor.ModelViewer.MemberInfoDrawer;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MVC.Editor.ModelViewer
{
    internal class InspectWindow : EditorWindow
    {
        private object _inspectedObject;
        private object _inspectedObjectContext;
        
        private Dictionary<MemberInfo, MemberInfoDrawerBase> _activePropertyDrawersDict;
        
        public void Initialize(object inspectedObject, object inspectedObjectContext)
        {
            _inspectedObject = inspectedObject;
            _inspectedObjectContext = inspectedObjectContext;
            
            _activePropertyDrawersDict = new Dictionary<MemberInfo, MemberInfoDrawerBase>();
        }

        private void OnGUI()
        {
            if(_inspectedObject == null)
                return;
            
            DisplayObjectFields(_inspectedObject);
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
                Type propertyDrawerType = ModelViewerUtils.GetMemberInfoDrawerType(memberInfo.GetMemberType());

                memberInfoDrawer = (MemberInfoDrawerBase) Activator.CreateInstance(propertyDrawerType, memberInfo, rootObject);
                _activePropertyDrawersDict.Add(memberInfo, memberInfoDrawer);
            }
            else
                memberInfoDrawer = _activePropertyDrawersDict[memberInfo];
            
            return memberInfoDrawer;
        }

        private void OnDestroy()
        {
            _activePropertyDrawersDict = new Dictionary<MemberInfo, MemberInfoDrawerBase>();
        }
    }
}