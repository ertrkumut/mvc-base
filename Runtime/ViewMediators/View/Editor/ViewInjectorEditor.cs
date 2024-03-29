﻿#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Root;
using MVC.Runtime.ViewMediators.Utils;
using MVC.Runtime.ViewMediators.View.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Editor
{
    [CustomEditor(typeof(ViewInjector))]
    [CanEditMultipleObjects]
    public class ViewInjectorEditor : UnityEditor.Editor
    {
        private Vector2 _guiScrollValue;
        
        private List<IView> _viewList;
        private ViewInjector _target;
        
        private void OnEnable()
        {
            _target = target as ViewInjector;
            if (_target.viewDataList == null)
                _target.viewDataList = new List<ViewInjectorData>();
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            FindViews();
            CreateOrDeleteViewInjectorData();
            DrawViewInjectorDataList();
        }

        private void FindViews()
        {
            _viewList = _target.GetComponents<IView>().ToList();
        }
        
        private void CreateOrDeleteViewInjectorData()
        {
            EditorGUI.BeginChangeCheck();
            var activeDataList = new List<ViewInjectorData>();

            foreach (var mvcView in _viewList)
            {
                var injectorData = _target.viewDataList.FirstOrDefault(x => x.View == (Object) mvcView);
                if(injectorData == null)
                {
                    injectorData = new ViewInjectorData
                    {
                        View = mvcView as Object,
                        AutoRegister = true
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
            if(Application.isPlaying)
                return;

            if (EditorGUI.EndChangeCheck())
            {
                var prefabScene = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabScene != null)
                {
                    EditorSceneManager.MarkSceneDirty(prefabScene.scene);
                }
                else if (PrefabUtility.IsOutermostPrefabInstanceRoot(_target.gameObject))
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(_target);
                }
                else
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        private void DrawViewInjectorDataList()
        {
            if (_viewList == null || _viewList.Count == 0)
            {
                EditorGUILayout.LabelField("There is no MVCView");
                return;
            }

            EditorGUI.BeginChangeCheck();

            _guiScrollValue = EditorGUILayout.BeginScrollView(_guiScrollValue);
            
            foreach (var viewInjectorData in _target.viewDataList)
            {
                ViewGUI(viewInjectorData);
            }
            
            EditorGUILayout.EndScrollView();
            
            MarkDirty();
        }

        private void ViewGUI(ViewInjectorData viewInjectorData)
        {
            EditorGUILayout.BeginVertical("box");

            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(viewInjectorData.View.GetType().Name, EditorStyles.boldLabel);
            EditorGUILayout.Space(3);
            EditorGUILayout.ObjectField(viewInjectorData.View, typeof(Object), false);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            viewInjectorData.AutoRegister = EditorGUILayout.Toggle(new GUIContent("Auto Register"), viewInjectorData.AutoRegister);
            viewInjectorData.InjectableView = EditorGUILayout.Toggle(new GUIContent("Injectable View"), viewInjectorData.InjectableView);
            
            viewInjectorData.UseBubbleUp = EditorGUILayout.Toggle(new GUIContent("Use Bubble-up"), viewInjectorData.UseBubbleUp);

            if (!viewInjectorData.UseBubbleUp)
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField(viewInjectorData.SelectedRoot, typeof(RootBase), false);
                GUI.enabled = true;

                if (GUILayout.Button("Select Context"))
                {
                    var genericMenu = new GenericMenu();

                    var contexts = FindObjectsOfType<RootBase>().ToList();
                    foreach (var context in contexts)
                    {
                        genericMenu.AddItem(new GUIContent(context.gameObject.name), false, root =>
                        {
                            viewInjectorData.SelectedRoot = root as RootBase;
                        }, context);
                    }
                    
                    genericMenu.ShowAsContext();
                }
            }
            else
                viewInjectorData.SelectedRoot = null;
            
            GUI.enabled = false;
            EditorGUILayout.Toggle(new GUIContent("Is Registered"), viewInjectorData.IsRegistered);
            GUI.enabled = true;
            
            EditorGUILayout.EndVertical();

            if (Application.isPlaying)
            {
                EditorGUILayout.BeginVertical();
                GUI.backgroundColor = Color.red;
                GUI.enabled = viewInjectorData.IsRegistered;
                var removeButton = GUILayout.Button("Remove Registration");
                GUI.backgroundColor = Color.white;
            
                if (removeButton)
                {
                    (viewInjectorData.View as IView).UnRegister();
                }

                GUI.enabled = !viewInjectorData.IsRegistered;
                GUI.backgroundColor = Color.green;
                var registerButton = GUILayout.Button("Register");
                if (registerButton)
                {
                    (viewInjectorData.View as IView).Register();
                }
                
                GUI.backgroundColor = Color.white;
                GUI.enabled = true;
            
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
                
            EditorGUILayout.EndVertical();
        }
    }
}
#endif