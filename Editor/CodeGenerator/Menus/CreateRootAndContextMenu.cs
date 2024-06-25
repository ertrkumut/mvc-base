#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MVC.Runtime.Root.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateRootAndContextMenu : EditorWindow
    {
        private static string _rootPath;

        private string _baseInput => !_isTest ? _rootPath.Split('/')[_rootPath.Split('/').Length - 1] : "TEST_" + _rootPath.Split('/')[_rootPath.Split('/').Length - 1]; 
        
        private string _rootName => _baseInput + "Root";
        private string _contextName => _baseInput + "Context";

        private bool _createScene = true;
        private bool _createRoot = true;
        private bool _isScreenContext = false;
        private bool _isTest;
        
        private bool _customScenePath;
        private string _scenePath;

        private string _parentFolderName;
        
        private Dictionary<string, bool> _parentFolderTogglesState;
        private MVCCodeGenerationSettings _codeGenerationSettings;
        
        private void OnEnable()
        {
            _rootPath = "*Name*";
            _parentFolderTogglesState = new Dictionary<string, bool>();

            _codeGenerationSettings = Resources.Load<MVCCodeGenerationSettings>("MVCCodeGenerationSettings");
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
            
            _isScreenContext = EditorGUILayout.ToggleLeft(new GUIContent("Is Screen"), _isScreenContext);
            if (_isScreenContext)
                _isTest = false;
            else
                _isTest = EditorGUILayout.ToggleLeft(new GUIContent("Is Test"), _isTest);
            
            _createRoot = EditorGUILayout.ToggleLeft(new GUIContent("Create Root"), _createRoot);

            if (!_createRoot)
            {
                _createScene = false;
                _customScenePath = false;
            }
            else
            {
                _createScene = EditorGUILayout.ToggleLeft(new GUIContent("Create Scene"), _createScene);
                _customScenePath = EditorGUILayout.ToggleLeft(new GUIContent("Custom Scene Path"), _customScenePath);
            }
            
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
                    if (!Directory.Exists(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ContextScenePath, _parentFolderName)))
                        Directory.CreateDirectory(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ContextScenePath, _parentFolderName));

                    _scenePath =
                        EditorUtility.OpenFolderPanel("Choose Scene Path", CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ContextScenePath, _parentFolderName), "") + "/";
                }
            }
            
            //--------------- Draw ParentFolder list ---------------
            if (_codeGenerationSettings != null || _codeGenerationSettings.ParentFolderNames.Count != 0)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Select Parent Folder");
                foreach (var parentFolder in _codeGenerationSettings.ParentFolderNames)
                {
                    if(string.IsNullOrEmpty(parentFolder))
                        continue;

                    _parentFolderTogglesState.TryAdd(parentFolder, false);

                    var selectionValue = EditorGUILayout.ToggleLeft(new GUIContent(parentFolder),
                        _parentFolderTogglesState[parentFolder]);

                    if (!selectionValue && _parentFolderName == parentFolder)
                    {
                        _parentFolderName = string.Empty;
                        _parentFolderTogglesState[parentFolder] = false;
                        continue;
                    }

                    if (selectionValue)
                    {
                        if (!string.IsNullOrEmpty(_parentFolderName))
                            _parentFolderTogglesState[_parentFolderName] = false;

                        _parentFolderName = parentFolder;
                        _parentFolderTogglesState[parentFolder] = true;
                    }
                }
            }
            //------------------------------------------------------

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            
            #endregion

            #region Create

            EditorGUILayout.Space(25);

            GUI.backgroundColor = Color.gray;

            GUI.enabled = true;

            if (string.IsNullOrEmpty(_parentFolderName))
            {
                GUI.enabled = true;
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Choose parent folder path!", MessageType.Error);
                GUI.enabled = false;
            }
            
            if (_customScenePath && string.IsNullOrEmpty(_scenePath))
            {
                GUI.enabled = true;
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Choose scene path!", MessageType.Error);
                GUI.enabled = false;
            }
            
            if (string.IsNullOrEmpty(_baseInput) || _baseInput == "*Name*")
            {
                GUI.enabled = true;
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Invalid Root Name!", MessageType.Error);
                GUI.enabled = false;
            }

            var createButton = GUILayout.Button("Create");
            GUI.backgroundColor = Color.white;
            
            if(createButton)
                CreateRootAndContext(_parentFolderName);

            #endregion
        }

        private void CreateRootAndContext(string parentFolderName)
        {
            var contextPath = Application.dataPath + string.Format(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ContextPath, parentFolderName), _rootPath);;
            if(_isTest)
                contextPath = Application.dataPath + string.Format(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TestContextPath, parentFolderName), _rootPath);;
            
            var namespaceText = contextPath.Replace(Application.dataPath + "/Scripts/", "").Replace("/", ".").TrimEnd('.');
            
            if(_isTest)
            {
                namespaceText = contextPath
                    .Replace(Application.dataPath + "/Test/Scripts/", "")
                    .Replace("/", ".")
                    .TrimEnd('.');

                namespaceText = "Test." + namespaceText;
            }

            CreateScene(parentFolderName);

            if(_isScreenContext)
                CodeGeneratorUtils.CreateContext(_contextName, "TempScreenContext", contextPath,
                    CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenContextPath, parentFolderName), namespaceText, true, false);
            else
                CodeGeneratorUtils.CreateContext(_contextName, "TempContext", contextPath,
                    CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempContextPath, parentFolderName), namespaceText, _isScreenContext, _isTest);
            
            if(_createRoot)
            {
                if(_isScreenContext)
                    CodeGeneratorUtils.CreateRoot(_rootName, _contextName, "TempScreenContext", "TempScreenRoot", contextPath,
                        CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenRootPath, parentFolderName), namespaceText, false);
                else
                    CodeGeneratorUtils.CreateRoot(_rootName, _contextName, "TempContext", "TempRoot", contextPath,
                        CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempRootPath, parentFolderName), namespaceText, _isTest);
            }
            _rootPath = "*Name*";
        }

        private void CreateScene(string parentFolderName)
        {
            if(!_createScene)
                return;

            var scenePath = _customScenePath 
                ? _scenePath : _isTest 
                    ? CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TestContextScenePath, parentFolderName) 
                    : CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ContextScenePath, parentFolderName);
            
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

            try
            {
                var rootName = PlayerPrefs.GetString("create-root-menu-clicked");
                var path = PlayerPrefs.GetString("create-root-scene-path") + rootName + ".unity";
            
                PlayerPrefs.DeleteKey("create-root-menu-clicked");
                PlayerPrefs.DeleteKey("create-root-scene-path");
            
                var rootGameObject = new GameObject(rootName);
                var rootType = AssemblyExtensions
                    .GetAllRootTypes()
                    .FirstOrDefault(x => x.Name == rootName);
            
                rootGameObject.AddComponent(rootType);

                var relativeScenePath = path.Replace(Application.dataPath, "");
                relativeScenePath = "Assets/" + relativeScenePath;

                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), relativeScenePath);

                AssetDatabase.Refresh();
            }
            catch (Exception)
            {
                PlayerPrefs.DeleteKey("create-root-menu-clicked");
                PlayerPrefs.DeleteKey("create-root-scene-path");
            }
        }
    }
}
#endif