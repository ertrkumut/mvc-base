#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MVC.Runtime.Contexts;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateViewMenu : EditorWindow
    {
        protected virtual string _classLabelName => "View Name: ";
        protected virtual string _classViewName => "View";
        protected virtual string _classMediatorName => "Mediator";

        protected virtual string _tempViewName => "TempView";
        protected virtual string _tempMediatorName => "TempMediator";
        
        protected virtual string _targetViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ViewPath);
        protected virtual string _tempViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempViewPath);
        protected virtual string _tempMediatorPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempMediatorPath);

        protected string _fileName;
        protected string _viewPath = "*Name*";

        protected List<string> _actionNames;

        protected string _viewNamespace;
        protected string _viewName;
        protected string _mediatorName;

        protected string _selectedContextName;
        private Dictionary<string, bool> _contextGUI;

        protected bool _isTestView;
        
        protected virtual void OnEnable()
        {
            _actionNames = new List<string>();
            _contextGUI = new Dictionary<string, bool>();

            _selectedContextName = "";
        }

        protected virtual void OnGUI()
        {
            #region ViewName

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(_classLabelName, GUILayout.Width(75));
            _viewPath = EditorGUILayout.TextField(_viewPath);
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();

            var fullName = _viewPath + _classViewName;
            
            if(GetType() == typeof(CreateViewMenu))
            {
                EditorGUILayout.LabelField("Is Test", GUILayout.Width(40));
                _isTestView = EditorGUILayout.Toggle(_isTestView, GUILayout.Width(30));
                
                if (_isTestView)
                    fullName = "TEST_" + fullName;
            }
                
            EditorGUILayout.LabelField(fullName, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            #endregion

            #region Actions

            EditorGUILayout.Space(25);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);

            GUI.backgroundColor = Color.green;
            var addActionButton = GUILayout.Button("Add Action");
            GUI.backgroundColor = Color.white;
            
            if(addActionButton)
                _actionNames.Add("OnActionName");

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

            DrawAllContexts();

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
            _viewName = _viewPath.Split('/')[_viewPath.Split('/').Length - 1] + _classViewName;
            _mediatorName = _viewPath.Split('/')[_viewPath.Split('/').Length - 1] + _classMediatorName;

            if (_isTestView)
            {
                _viewName = "TEST_" + _viewName;
                _mediatorName = "TEST_" + _mediatorName;

                _viewPath = "TEST/" + _viewPath;
            }
         
            var path = Application.dataPath + string.Format(_targetViewPath, _selectedContextName.Replace("Context", "")) + _viewPath;
            _viewNamespace = path.Replace(Application.dataPath + "/Scripts/", "").Replace("/", ".").TrimEnd('.');
            
            CodeGeneratorUtils.CreateView(_viewName, _tempViewName, path, _tempViewPath, _viewNamespace, _actionNames, _isTestView);
            CodeGeneratorUtils.CreateMediator(_mediatorName, _viewName, _tempMediatorName, path, _tempMediatorPath, _viewNamespace, _actionNames, _isTestView);

            _fileName = _viewName;
            _viewPath = "*Name*";
            _fileName = "";
        }

        private void DrawAllContexts()
        {
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            var currentAssembly = assemblyList.FirstOrDefault(x => x.FullName.StartsWith("Assembly-CSharp,"));
            var contextTypes = currentAssembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Context)))
                .ToList()
                .Select(x => x.Name)
                .ToList();

            EditorGUILayout.BeginVertical("box");
            for (var ii = 0; ii < contextTypes.Count; ii++)
            {
                EditorGUILayout.BeginHorizontal();
                var contextName = contextTypes[ii];
                
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
                }
                else if (!result && _selectedContextName == contextName)
                {
                    _selectedContextName = "";
                }
                
                _contextGUI[contextName] = result;
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
#endif