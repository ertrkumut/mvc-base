#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Runtime.Root.Utils;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateViewMenu : EditorWindow
    {
        protected virtual bool _showCreateSceneToggle => false;
        protected bool _createScene = true;
        
        protected virtual string _classLabelName => "View Name: ";
        protected virtual string _classViewName => "View";
        protected virtual string _classMediatorName => "Mediator";

        protected virtual string _tempViewName => "TempView";
        protected virtual string _tempMediatorName => "TempMediator";
        
        protected virtual string _targetViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ViewPath, _parentFolderName);
        protected virtual string _targetTestViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TestViewPath, _parentFolderName);
        protected virtual string _tempViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempViewPath, _parentFolderName);
        protected virtual string _tempMediatorPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempMediatorPath, _parentFolderName);

        protected string _fileName;
        protected string _viewNameInputField = "*Name*";

        protected List<string> _actionNames;

        protected string _parentFolderName;

        protected string _viewNamespace
        {
            get
            {
                return _viewCreationPath.Replace(Application.dataPath + "/Scripts/", "")
                    .Replace("/", ".")
                    .TrimEnd('.');
            }
        }
        protected string _viewName;
        protected string _mediatorName;

        protected string _selectedContextName;
        private Dictionary<string, bool> _contextGUI;

        protected bool _isTestView;
        
        protected List<Type> _contextTypes;

        protected string _viewCreationPath
        {
            get
            {
                if (string.IsNullOrEmpty(_selectedContextName))
                    return "";

                var contextType = _contextTypes.FirstOrDefault(x => x.Name == _selectedContextName);

                var pureContextFolder = contextType.Namespace.Substring(contextType.Namespace.IndexOf("Contexts.") + 9)
                    .Replace(".Root", "")
                    .Replace(".","/");
                
                return Application.dataPath  
                       + string.Format(_targetViewPath, pureContextFolder) 
                       + _viewNameInputField;
            }
        }

        protected string _viewCreationPathForDEBUG
        {
            get
            {
                var path = _viewCreationPath;
                if (string.IsNullOrEmpty(path))
                    return "";

                var index = path.IndexOf("Scripts");
                return path.Substring(index);
            }
        }
        
        protected virtual void OnEnable()
        {
            _actionNames = new List<string>();
            _contextGUI = new Dictionary<string, bool>();

            _selectedContextName = "";

            _contextTypes = AssemblyExtensions.GetAllContextTypes();
        }

        protected virtual void OnGUI()
        {
            #region ViewName

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(_classLabelName, GUILayout.Width(75));
            _viewNameInputField = EditorGUILayout.TextField(_viewNameInputField);
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();

            var fullName = _viewNameInputField + _classViewName;
            
            if(GetType() == typeof(CreateViewMenu))
            {
                EditorGUILayout.LabelField("Is Test", GUILayout.Width(40));
                var tempTestStatus = EditorGUILayout.Toggle(_isTestView, GUILayout.Width(30));

                if ((_isTestView && !tempTestStatus) || (!_isTestView && tempTestStatus))
                {
                    _selectedContextName = "";
                    _contextGUI = new Dictionary<string, bool>();
                }

                _isTestView = tempTestStatus;
                
                if (_isTestView)
                    fullName = "TEST_" + fullName;
            }
                
            EditorGUILayout.LabelField(fullName, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            #endregion

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"View Path: {_viewCreationPathForDEBUG}");
            EditorGUILayout.LabelField($"View Namespace: {_viewNamespace}");
            
            if(_showCreateSceneToggle)
                _createScene = EditorGUILayout.ToggleLeft(new GUIContent("Create Scene (Prefab won't be created if it's false)"), _createScene);
            
            #region Actions

            EditorGUILayout.Space(25);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);

            GUI.backgroundColor = Color.green;
            var addActionButton = GUILayout.Button("Add Action");
            GUI.backgroundColor = Color.white;
            
            if(addActionButton)
                _actionNames.Add("ActionName");

            for (var ii = 0; ii < _actionNames.Count; ii++)
            {
                EditorGUILayout.BeginHorizontal("box");

                _actionNames[ii] = EditorGUILayout.TextField(_actionNames[ii]);
                GUI.backgroundColor = Color.red;
                var removeButton = GUILayout.Button("-", GUILayout.Width(75));
                GUI.backgroundColor = Color.white;
                
                if (removeButton)
                {
                    _actionNames.RemoveAt(ii);
                    return;
                }
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            #endregion

            #region SelectContext

            GUI_ContextList();

            #endregion
            
            #region Create
            
            EditorGUILayout.Space(25);

            GUI.enabled = !string.IsNullOrEmpty(_selectedContextName);
            if(!GUI.enabled)
            {
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Choose Context!", MessageType.Error);
            }
            GUI.backgroundColor = Color.gray;
            var createButton = GUILayout.Button("Create");
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;
            if(createButton)
                CreateViewMediator();

            #endregion
        }

        protected virtual void CreateViewMediator()
        {
            _viewName = _viewNameInputField.Split('/')[_viewNameInputField.Split('/').Length - 1] + _classViewName;
            _mediatorName = _viewNameInputField.Split('/')[_viewNameInputField.Split('/').Length - 1] + _classMediatorName;

            if (_isTestView)
            {
                _viewName = "TEST_" + _viewName;
                _mediatorName = "TEST_" + _mediatorName;
            }
            
            CodeGeneratorUtils.CreateView(_viewName, _tempViewName, _viewCreationPath, _tempViewPath, _viewNamespace, _actionNames, _isTestView);
            CodeGeneratorUtils.CreateMediator(_mediatorName, _viewName, _tempMediatorName, _viewCreationPath, _tempMediatorPath, _viewNamespace, _actionNames, _isTestView);

            _fileName = _viewName;
        }

        protected virtual void GUI_ContextList()
        {
            DrawAllContexts();
        }

        protected Vector2 _contextsScrollView = Vector2.zero; // To track the scroll position
        protected string _contextSearchText = ""; // To hold the search text

        
        protected void DrawAllContexts(bool isScreen = false)
        {
            var contextResultList = new List<Type>();
            
            if (isScreen)
                contextResultList = _contextTypes
                    .Where(x => x.GetField(CodeGeneratorStrings.ContextScreenFlag, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic) != null)
                    .ToList();
            else
            {
                if (_isTestView)
                    contextResultList = _contextTypes
                        .Where(x => x.GetField(CodeGeneratorStrings.ContextTestFlag, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic) != null)
                        .ToList();
                else
                    contextResultList = _contextTypes
                        .Where(x => x.GetField(CodeGeneratorStrings.ContextTestFlag, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic) == null)
                        .ToList();
            }
                
            var contextNames = contextResultList
                .Select(x => x.Name)
                .ToList();

            EditorGUILayout.BeginVertical("box");
            
            var searchStyle = new GUIStyle(GUI.skin.textField);
            searchStyle.fontSize = 14;
            searchStyle.normal.textColor = Color.white;
            searchStyle.alignment = TextAnchor.MiddleLeft;
            searchStyle.padding = new RectOffset(10, 0, 0, 0);
            
            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fontSize = 12;
            labelStyle.normal.textColor = Color.gray;
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search:", labelStyle, GUILayout.Width(50)); // Adjust width as needed
            _contextSearchText = EditorGUILayout.TextField(_contextSearchText, searchStyle, GUILayout.MinWidth(200), GUILayout.Height(17));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            _contextsScrollView = EditorGUILayout.BeginScrollView(_contextsScrollView);
            
            for (var ii = 0; ii < contextNames.Count; ii++)
            {
                EditorGUILayout.BeginHorizontal();
                var contextName = contextNames[ii];
                
                if(!string.IsNullOrEmpty(_contextSearchText) && !contextName.Contains(_contextSearchText))
                {
                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                
                if(!_contextGUI.ContainsKey(contextName))
                    _contextGUI.Add(contextName, false);
                
                var result = EditorGUILayout.ToggleLeft(contextName, _contextGUI[contextName]);
                if (result && _selectedContextName != contextName)
                {
                    if (!string.IsNullOrEmpty(_selectedContextName))
                    {
                        _contextGUI[_selectedContextName] = false;
                    }
                    _selectedContextName = contextName;
                    _parentFolderName = contextResultList.FirstOrDefault(x => x.Name == contextName).Namespace.Split(".")[0];
                }
                else if (!result && _selectedContextName == contextName)
                {
                    _selectedContextName = "";
                }
                
                _contextGUI[contextName] = result;
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif