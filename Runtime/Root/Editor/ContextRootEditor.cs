#if UNITY_EDITOR
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
            GUI_InitializeOrder();
            GUI_BindingOptions();
            GUI_InitializeOptions();
            GUI_LaunchOptions();

            GUI_RootStatus();
        }

        private void GUI_InitializeOrder()
        {
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.BeginVertical("box");
            
            var initializeOrder = EditorGUILayout.IntField("Initialize Order: ", _root.initializeOrder);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "initialize-order");
                _root.initializeOrder = initializeOrder;
                if(!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            EditorGUILayout.EndVertical();
        }

        private void GUI_BindingOptions()
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUI.BeginChangeCheck();
            
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

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_root, "binding-flags");
                _root.autoBindSignals = signalBinding;
                _root.autoBindInjections = injectionBinding;
                _root.autoBindMediations = mediationBinding;
                _root.autoBindCommands = commandBinding;
                
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

            var autoInitialize = EditorGUILayout.ToggleLeft("Auto Initialize", _root.autoInitialize, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.hasInitialized);

            if (GUI.enabled && !_root.hasInitialized)
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
                _root.autoInitialize = autoInitialize;
                
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

            var autoLaunch = EditorGUILayout.ToggleLeft("Auto Launch", _root.autoLaunch, GUILayout.Width(125));
            GUI.enabled = (Application.isPlaying && !_root.hasLaunched && _root.hasInitialized);

            if (GUI.enabled && !_root.hasLaunched)
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
                _root.autoLaunch = autoLaunch;
                
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
            
            guiStyle.normal.textColor = _root.signalsBound ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Signals Bound: " + _root.signalsBound, guiStyle);

            guiStyle.normal.textColor = _root.injectionsBound ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Injections Bound: " + _root.injectionsBound, guiStyle);

            guiStyle.normal.textColor = _root.mediationsBound ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Mediations Bound: " + _root.mediationsBound, guiStyle);
            
            guiStyle.normal.textColor = _root.commandsBound ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Commands Bound: " + _root.commandsBound, guiStyle);

            EditorGUILayout.Separator();
            
            guiStyle.normal.textColor = _root.hasInitialized ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Has Initialized: " + _root.hasInitialized, guiStyle);
            
            guiStyle.normal.textColor = _root.hasLaunched ? Color.green : Color.red;            
            EditorGUILayout.LabelField("Has Launched: " + _root.hasLaunched, guiStyle);
            
            EditorGUILayout.EndVertical();
        }
    }
}
#endif