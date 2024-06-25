#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.CodeGenerator;
using MVC.Runtime.Contexts;
using MVC.Runtime.Root;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Root.Editor
{
    [CustomEditor(typeof(RootBase), true)]
    public class ContextRootEditor : UnityEditor.Editor
    {
        private RootBase _root;

        private void OnEnable()
        {
            _root = target as RootBase;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(10);
            
            GUI_InitializeOrder();
            GUI_BindingOptions();
            GUI_InitializeOptions();
            GUI_SetupOptions();
            GUI_LaunchOptions();

            GUI_RootStatus();

            GUI_SubContexts();
        }

        private void GUI_InitializeOrder()
        {
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.BeginVertical("box");
            
            var initializeOrder = EditorGUILayout.IntField("Initialize Order: ", _root.InitializeOrder);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "initialize-order");
                _root.InitializeOrder = initializeOrder;
                if(!Application.isPlaying)
                    MarkDirty();
            }
            
            EditorGUILayout.EndVertical();
        }

        private void GUI_BindingOptions()
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUI.BeginChangeCheck();
            
            #region Injection

            EditorGUILayout.BeginHorizontal();
            
            var injectionBinding = EditorGUILayout.ToggleLeft("Bind Injections", _root.AutoBindInjections, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.InjectionsBound);

            if (GUI.enabled && !_root.InjectionsBound)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var injectionButton = GUILayout.Button("Bind Injections");
            if(injectionButton)
                _root.BindInjections(true);
            
            GUI.backgroundColor = Color.white;
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Mediation

            EditorGUILayout.BeginHorizontal();
            
            var mediationBinding = EditorGUILayout.ToggleLeft("Bind Mediations", _root.AutoBindMediations, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.MediationsBound);

            if (GUI.enabled && !_root.MediationsBound)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var mediationButton = GUILayout.Button("Bind Mediations");
            if(mediationButton)
                _root.BindMediations(true);
            
            GUI.backgroundColor = Color.white;
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            #endregion

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "binding-flags");
                _root.AutoBindInjections = injectionBinding;
                _root.AutoBindMediations = mediationBinding;

                if(!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }            
            EditorGUILayout.EndVertical();
        }

        private void GUI_InitializeOptions()
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            var autoInitialize = EditorGUILayout.ToggleLeft("Auto Initialize", _root.AutoInitialize, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.HasInitialized);

            if (GUI.enabled && !_root.HasInitialized)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var launchButton = GUILayout.Button("Initialize");
            if(launchButton)
                _root.StartContext(true);
            
            GUI.backgroundColor = Color.white;
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "auto-initialize");
                _root.AutoInitialize = autoInitialize;
                
                if(!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        
        private void GUI_SetupOptions()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            var autoSetup = EditorGUILayout.ToggleLeft("Auto Setup", _root.autoSetup, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.hasSetuped && _root.hasInitialized);

            if (GUI.enabled && !_root.hasSetuped)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var setupButton = GUILayout.Button("Setup");
            if(setupButton)
                _root.Setup(true);
            
            GUI.backgroundColor = Color.white;
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "auto-setup");
                _root.autoSetup = autoSetup;
                
                if(!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        private void GUI_LaunchOptions()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            var autoLaunch = EditorGUILayout.ToggleLeft("Auto Launch", _root.AutoLaunch, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.HasLaunched && _root.HasInitialized);

            if (GUI.enabled && !_root.HasLaunched)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var launchButton = GUILayout.Button("Launch");
            if(launchButton)
                _root.Launch(true);
            
            GUI.backgroundColor = Color.white;
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "auto-launch");
                _root.AutoLaunch = autoLaunch;
                
                if(!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        private void GUI_RootStatus()
        {
            var guiStyle = new GUIStyle(EditorStyles.textField);
            
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.LabelField("Context Status:");
            
            guiStyle.normal.textColor = _root.InjectionsBound ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Injections Bound: " + _root.InjectionsBound, guiStyle);

            guiStyle.normal.textColor = _root.MediationsBound ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Mediations Bound: " + _root.MediationsBound, guiStyle);

            EditorGUILayout.Separator();
            
            guiStyle.normal.textColor = _root.HasInitialized ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Has Initialized: " + _root.HasInitialized, guiStyle);
            
            guiStyle.normal.textColor = _root.HasLaunched ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Has Launched: " + _root.HasLaunched, guiStyle);
            
            EditorGUILayout.EndVertical();
        }
        
        private void GUI_SubContexts()
        {
            if (_root.SubContextTypes == null)
                _root.SubContextTypes = new List<SubContextData>();
            
            if (_root.SubContextTypes.Count != 0)
            {
                EditorGUILayout.BeginVertical("box");

                for (var ii = 0; ii < _root.SubContextTypes.Count; ii++)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.BeginHorizontal();

                    var contextData = _root.SubContextTypes[ii];
                    EditorGUILayout.LabelField(contextData.ContextName);

                    if (GUILayout.Button("-"))
                    {
                        _root.SubContextTypes.RemoveAt(ii);
                        MarkDirty();
                        return;
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.BeginChangeCheck();
                    
                    var contextDataAutoSetup =
                        EditorGUILayout.Toggle(new GUIContent("AutoSetup"), contextData.AutoSetup);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_root, "auto-setup");
                        contextData.AutoSetup = contextDataAutoSetup;
                        
                        if(!Application.isPlaying)
                            MarkDirty();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(5);
                }
                
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Sub Context"))
            {
                AddSubContextWindow.ShowWindow(_root);
            }
        }

        private void MarkDirty()
        {
            var prefabScene = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabScene != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabScene.scene);
            }
            else if (PrefabUtility.IsOutermostPrefabInstanceRoot(_root.gameObject))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(_root);
            }
            else
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif