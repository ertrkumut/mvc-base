using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateViewMenu : EditorWindow
    {
        protected virtual string _classLabelName => "View Name: ";
        protected virtual string _classViewName => "View";
        protected virtual string _classMediatorName => "Mediator";
        protected virtual string _namespace => "Runtime.Views.";

        protected virtual string _tempViewName => "TempView";
        protected virtual string _tempMediatorName => "TempMediator";
        
        protected virtual string _targetViewPath => CodeGeneratorStrings.ViewPath;
        protected virtual string _tempViewPath => CodeGeneratorStrings.TempViewPath;
        protected virtual string _tempMediatorPath => CodeGeneratorStrings.TempMediatorPath;
        
        
        protected string _viewPath = "*Name*";

        protected List<string> _actionNames;

        protected virtual void OnEnable()
        {
            _actionNames = new List<string>();
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
            
            GUILayout.Space(80);
            EditorGUILayout.LabelField(_viewPath + _classViewName, EditorStyles.boldLabel);
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

            #region Create
            
            EditorGUILayout.Space(25);

            GUI.backgroundColor = Color.gray;
            var createButton = GUILayout.Button("Create");
            GUI.backgroundColor = Color.white;
            
            if(createButton)
                CreateViewMediator();

            #endregion
        }

        protected virtual void CreateViewMediator()
        {
            var path = Application.dataPath + _targetViewPath + _viewPath;
            var viewName = _viewPath.Split('/')[_viewPath.Split('/').Length - 1] + _classViewName;
            var mediatorName = _viewPath.Split('/')[_viewPath.Split('/').Length - 1] + _classMediatorName;
            
            var namespaceText = _namespace + _viewPath.Replace("/",".");
            
            CreateView(path, viewName, namespaceText);
            CreateMediator(path, mediatorName, viewName, namespaceText);
        }

        protected virtual void CreateView(string path, string fileName, string namespaceText)
        {
            var newViewPath = path + "/" + fileName + ".cs";
            var tempViewClassPath = _tempViewPath;

            var tempViewContent = File.ReadAllLines(tempViewClassPath);
            var newViewContent = new List<string>();
            
            for (var ii = 0; ii < tempViewContent.Length; ii++)
            {
                var content = tempViewContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceText;
                }
                else if (content.Contains("internal class "))
                {
                    content = content.Replace("internal class", "public class");
                    content = content.Replace(_tempViewName, fileName);
                }
                else if (content.Contains("//@Actions"))
                {
                    foreach (var actionName in _actionNames)
                    {
                        newViewContent.Add("\t\tpublic Action " + actionName + ";");
                    }
                    continue;
                }
                
                newViewContent.Add(content);
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            File.WriteAllLines(newViewPath, newViewContent.ToArray());
            AssetDatabase.Refresh();
        }

        protected virtual void CreateMediator(string path, string fileName, string viewName, string namespaceText)
        {
            var newMediatorPath = path + "/" + fileName + ".cs";
            var tempMediatorClassPath = _tempMediatorPath;

            var tempMediatorContent = File.ReadAllLines(tempMediatorClassPath);
            var newMediatorContent = new List<string>();
            
            for (var ii = 0; ii < tempMediatorContent.Length; ii++)
            {
                var content = tempMediatorContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceText;
                }
                else if (content.Contains("internal class "))
                {
                    content = content.Replace("internal class", "public class");
                    content = content.Replace(_tempMediatorName, fileName);
                }
                else if (content.Contains("[Inject]"))
                {
                    content = "\t\t[Inject] private " + viewName + " _view { get; set; }";
                }
                else if (content.Contains("//@Register"))
                {
                    foreach (var actionName in _actionNames)
                    {
                        var line = "\t\t\t_view." + actionName + " += " + actionName + "Listener;";
                        newMediatorContent.Add(line);
                    }
                    continue;
                }
                else if (content.Contains("//@Remove"))
                {
                    foreach (var actionName in _actionNames)
                    {
                        var line = "\t\t\t_view." + actionName + " -= " + actionName + "Listener;";
                        newMediatorContent.Add(line);
                    }
                    continue;
                }
                else if (content.Contains("//@Methods"))
                {
                    foreach (var actionName in _actionNames)
                    {
                        var line = "\t\tprivate void " + actionName + "Listener()";
                        newMediatorContent.Add(line);
                        newMediatorContent.Add("\t\t{");
                        newMediatorContent.Add("\t\t}");
                        newMediatorContent.Add("");
                    }
                    newMediatorContent.RemoveAt(newMediatorContent.Count-1);
                    continue;
                }
                
                newMediatorContent.Add(content);
            }
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            File.WriteAllLines(newMediatorPath, newMediatorContent.ToArray());
            AssetDatabase.Refresh();
        }
    }
}