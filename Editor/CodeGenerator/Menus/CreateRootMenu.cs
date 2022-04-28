using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateRootMenu : EditorWindow
    {
        private static string _rootPath;

        private static string _baseInput => _rootPath.Split('/')[_rootPath.Split('/').Length - 1]; 
        
        private static string _rootName => _baseInput + "Root";
        private string _contextName => _baseInput + "Context";

        private bool _createScene = true;
        private bool _customScenePath;
        private string _scenePath;
        
        private void OnEnable()
        {
            _rootPath = "*Name*";
        }

        private void OnDisable()
        {
            _rootPath = "*Name*";
        }

        private void OnGUI()
        {
            #region RootName

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Root Name: ", GUILayout.Width(75));
            _rootPath = EditorGUILayout.TextField(_rootPath);
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Space(80);
            EditorGUILayout.LabelField(_rootName + "<" + _contextName + ">", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            
            _createScene = EditorGUILayout.ToggleLeft(new GUIContent("Create Scene"), _createScene);
            _customScenePath = EditorGUILayout.ToggleLeft(new GUIContent("Custom Scene Path"), _customScenePath);

            if (!_customScenePath)
                _scenePath = String.Empty;
            
            if (_customScenePath)
            {
                if (!String.IsNullOrEmpty(_scenePath))
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField("Path: " + _scenePath);
                    GUI.enabled = true;   
                }
                
                if (GUILayout.Button("Choose Path"))
                {
                    if (!Directory.Exists(CodeGeneratorStrings.RootScenePath))
                        Directory.CreateDirectory(CodeGeneratorStrings.RootScenePath);

                    _scenePath =
                        EditorUtility.OpenFolderPanel("Choose Scene Path", CodeGeneratorStrings.RootScenePath, "") + "/";
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            
            #endregion

            #region Create

            EditorGUILayout.Space(25);

            GUI.backgroundColor = Color.gray;

            GUI.enabled = true;
            
            if (_customScenePath && String.IsNullOrEmpty(_scenePath))
            {
                GUI.enabled = true;
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Choose scene path!", MessageType.Error);
                GUI.enabled = false;
            }
            
            if (String.IsNullOrEmpty(_baseInput) || _baseInput == "*Name*")
            {
                GUI.enabled = true;
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Invalid Root Name!", MessageType.Error);
                GUI.enabled = false;
            }

            var createButton = GUILayout.Button("Create");
            GUI.backgroundColor = Color.white;
            
            if(createButton)
                CreateRootAndContext();

            #endregion
        }

        private void CreateRootAndContext()
        {
            var namespaceText = "Runtime.Roots." + _rootPath.Replace("/",".");
            CreateScene();
            
            var contextPath = Application.dataPath + CodeGeneratorStrings.RootPath + _rootPath;
            
            CodeGeneratorUtils.CreateContext(_contextName, "TempContext", contextPath,
                CodeGeneratorStrings.TempContextPath, namespaceText);
            
            CodeGeneratorUtils.CreateRoot(_rootName, _contextName, "TempContext", "TempRoot", contextPath,
                CodeGeneratorStrings.TempRootPath, namespaceText);
        }

        private void CreateScene()
        {
            if(!_createScene)
                return;
            
            var scenePath = _customScenePath ? _scenePath : CodeGeneratorStrings.RootScenePath;
            
            PlayerPrefs.SetString("create-root-menu-clicked", _rootName);
            PlayerPrefs.SetString("create-root-scene-path", scenePath);

            if (!Directory.Exists(scenePath))
                Directory.CreateDirectory(scenePath);
            
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            scene.name = _rootName;
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CodeGenerationCompleted()
        {
            if(!PlayerPrefs.HasKey("create-root-menu-clicked"))
                return;
            
            var rootName = PlayerPrefs.GetString("create-root-menu-clicked");
            var path = PlayerPrefs.GetString("create-root-scene-path") + rootName + ".unity";
            
            PlayerPrefs.DeleteKey("create-root-menu-clicked");
            PlayerPrefs.DeleteKey("create-root-scene-path");
            
            var rootGameObject = new GameObject(rootName);
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            var currentAssembly = assemblyList.FirstOrDefault(x => x.FullName.StartsWith("Assembly-CSharp"));
            var rootType = currentAssembly.GetTypes().FirstOrDefault(x => x.Name == rootName);
            
            rootGameObject.AddComponent(rootType);

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), path);
            AssetDatabase.Refresh();
        }
    }
}