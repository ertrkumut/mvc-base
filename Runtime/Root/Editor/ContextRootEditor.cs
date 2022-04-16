using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Runtime.Root.Editor
{
    [CustomEditor(typeof(ContextRoot<>), true)]
    public class ContextRootEditor : UnityEditor.Editor
    {
        private RootBase _root;

        private void OnEnable()
        {
            _root = target as RootBase;
        }

        public override void OnInspectorGUI()
        {
            GUI_InitializeOrder();
            GUI_BindingOptions();
            GUI_LaunchOptions();
        }

        private void GUI_InitializeOrder()
        {
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.BeginVertical("box");
            
            var initializeOrder = EditorGUILayout.IntField("Initialize Order: ", _root.initializeOrder);

            if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
            {
                Undo.RecordObject(_root, "initialize-order");
                _root.initializeOrder = initializeOrder;
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            EditorGUILayout.EndVertical();
        }

        private void GUI_BindingOptions()
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUI.BeginChangeCheck();
            
            #region Signal
            
            EditorGUILayout.BeginHorizontal();
            
            var signalBinding = EditorGUILayout.ToggleLeft("Bind Signals", _root.autoBindSignals, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.signalsBound);

            if (GUI.enabled && !_root.signalsBound)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var signalButton = GUILayout.Button("Bind Signals");
            if(signalButton)
                _root.BindSignals(true);
            
            GUI.backgroundColor = Color.white;
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Injection

            EditorGUILayout.BeginHorizontal();
            
            var injectionBinding = EditorGUILayout.ToggleLeft("Bind Injections", _root.autoBindInjections, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.injectionsBound);

            if (GUI.enabled && !_root.injectionsBound)
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
            
            var mediationBinding = EditorGUILayout.ToggleLeft("Bind Mediations", _root.autoBindMediations, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.mediationsBound);

            if (GUI.enabled && !_root.mediationsBound)
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
            
            #region Commands

            EditorGUILayout.BeginHorizontal();
            
            var commandBinding = EditorGUILayout.ToggleLeft("Bind Commands", _root.autoBindCommands, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.commandsBound);

            if (GUI.enabled && !_root.commandsBound)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var commandButton = GUILayout.Button("Bind Commands");
            if(commandButton)
                _root.BindCommands(true);
            
            GUI.backgroundColor = Color.white;
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            #endregion

            if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
            {
                Undo.RecordObject(_root, "binding-flags");
                _root.autoBindSignals = signalBinding;
                _root.autoBindInjections = injectionBinding;
                _root.autoBindMediations = mediationBinding;
                _root.autoBindCommands = commandBinding;
                
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }            
            EditorGUILayout.EndVertical();
        }
        
        private void GUI_LaunchOptions()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            var autoLaunch = EditorGUILayout.ToggleLeft("Auto Launch", _root.autoLaunch, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.hasLaunched);

            if (GUI.enabled && !_root.hasLaunched)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.red;

            var launchButton = GUILayout.Button("Launch");
            if(launchButton)
                _root.Launch(true);
            
            GUI.backgroundColor = Color.white;
            
            if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
            {
                Undo.RecordObject(_root, "auto-launch");
                _root.autoLaunch = autoLaunch;
                
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
    }
}