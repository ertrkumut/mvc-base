#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.Utils;
using MVC.Runtime.ViewMediators.View.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Runtime.ViewMediators.View.Editor
{
    [UnityEditor.CustomEditor(typeof(ViewInjector))]
    [UnityEditor.CanEditMultipleObjects]
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
                var injectorData = _target.viewDataList.FirstOrDefault(x => x.view == (Object) mvcView);
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
            EditorGUILayout.LabelField(viewInjectorData.view.GetType().Name, EditorStyles.boldLabel);
            EditorGUILayout.Space(3);
            EditorGUILayout.ObjectField(viewInjectorData.view, typeof(Object), false);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            viewInjectorData.autoInject = EditorGUILayout.Toggle(new GUIContent("Auto Inject"), viewInjectorData.autoInject);
                
            GUI.enabled = false;
            EditorGUILayout.Toggle(new GUIContent("Is Injected"), viewInjectorData.isInjected);
            GUI.enabled = true;
            
            EditorGUILayout.EndVertical();

            if (Application.isPlaying)
            {
                EditorGUILayout.BeginVertical();
                GUI.backgroundColor = Color.red;
                GUI.enabled = viewInjectorData.isInjected;
                var removeButton = GUILayout.Button("Remove Registration");
                GUI.backgroundColor = Color.white;
            
                if (removeButton)
                {
                    (viewInjectorData.view as IView).RemoveRegistration();
                }

                GUI.enabled = !viewInjectorData.isInjected;
                GUI.backgroundColor = Color.green;
                var registerButton = GUILayout.Button("Register");
                if (registerButton)
                {
                    (viewInjectorData.view as IView).InjectView();
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