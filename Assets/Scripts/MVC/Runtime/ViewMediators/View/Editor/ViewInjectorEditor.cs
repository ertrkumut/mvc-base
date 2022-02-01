using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.View.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Editor
{
    [CustomEditor(typeof(ViewInjectorComponent))]
    [CanEditMultipleObjects]
    public class ViewInjectorEditor : UnityEditor.Editor
    {
        private List<IMVCView> _viewList;
        private ViewInjectorComponent _target;
        
        private void OnEnable()
        {
            _target = target as ViewInjectorComponent;
            if (_target.viewDataList == null)
                _target.viewDataList = new List<ViewInjectorData>();
        }

        public override void OnInspectorGUI()
        {
            FindViews();
            CreateOrDeleteViewInjectorData();
            DrawViewInjectorDataList();
        }

        private void FindViews()
        {
            _viewList = _target.GetComponents<IMVCView>().ToList();
        }
        
        private void CreateOrDeleteViewInjectorData()
        {
            EditorGUI.BeginChangeCheck();
            var activeDataList = new List<ViewInjectorData>();

            foreach (var mvcView in _viewList)
            {
                var injectorData = _target.viewDataList.FirstOrDefault(x => x.view == mvcView);
                if(injectorData == null)
                {
                    injectorData = new ViewInjectorData
                    {
                        view = mvcView as Object,
                        autoInject = true
                    };
                    _target.viewDataList.Add(injectorData);
                }
                
                activeDataList.Add(injectorData);
            }

            foreach (var viewInjectorData in _target.viewDataList)
            {
                if (!activeDataList.Contains(viewInjectorData))
                {
                    _target.viewDataList.Remove(viewInjectorData);
                    MarkDirty();
                    return;
                }
            }
            
            MarkDirty();
        }

        private void MarkDirty()
        {
            if (EditorGUI.EndChangeCheck())
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private void DrawViewInjectorDataList()
        {
            if (_viewList == null || _viewList.Count == 0)
            {
                EditorGUILayout.LabelField("There is no MVCView");
                return;
            }

            EditorGUI.BeginChangeCheck();
            
            foreach (var viewInjectorData in _target.viewDataList)
            {
                EditorGUILayout.BeginVertical("box");

                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(viewInjectorData.view.GetType().Name);
                EditorGUILayout.Space(3);
                EditorGUILayout.ObjectField(viewInjectorData.view, typeof(Object));

                EditorGUILayout.EndHorizontal();
                GUI.enabled = true;

                viewInjectorData.autoInject = EditorGUILayout.Toggle(new GUIContent("Auto Inject"), viewInjectorData.autoInject);
                
                GUI.enabled = false;
                EditorGUILayout.Toggle(new GUIContent("Is Injected"), viewInjectorData.isInjected);
                GUI.enabled = true;
                
                EditorGUILayout.EndVertical();
            }
            
            MarkDirty();
        }
    }
}