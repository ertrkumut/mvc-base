using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MVC.Runtime.CodeGenerator.Editor.Menus
{
    internal class CreateRootMenu : EditorWindow
    {
        private string _rootPath;

        private string _baseInput => _rootPath.Split('/')[_rootPath.Split('/').Length - 1]; 
        
        private string _rootName => _baseInput + "Root";
        private string _contextName => _baseInput + "Context";

        private bool _createScene = true;
        
        private void OnEnable()
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

            _createScene = EditorGUILayout.ToggleLeft(new GUIContent("Create Scene"), _createScene);
            EditorGUILayout.EndVertical();
            
            #endregion

            #region Create

            EditorGUILayout.Space(25);

            GUI.backgroundColor = Color.gray;
            var createButton = GUILayout.Button("Create");
            GUI.backgroundColor = Color.white;
            
            if(createButton)
                CreateRootAndContext();

            #endregion
        }

        private void CreateRootAndContext()
        {
            CreateContext();
            CreateRoot();
        }

        private void CreateContext()
        {
            var directoryPath = Application.dataPath + CodeGeneratorStrings.RootPath + _rootPath;
            var path = Application.dataPath + CodeGeneratorStrings.RootPath + _rootPath + "/" + _contextName + ".cs";

            var tempContextContent = File.ReadAllLines(CodeGeneratorStrings.TempContextPath);
            var newContextContent = new List<string>();
            
            var namespaceText = "Runtime.Roots." + _rootPath.Replace("/",".");
            
            for (var ii = 0; ii < tempContextContent.Length; ii++)
            {
                var content = tempContextContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceText;
                }
                else if (content.Contains("internal class "))
                {
                    content = "\tpublic class " + _contextName + " : Context";
                }

                newContextContent.Add(content);
            }
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            File.WriteAllLines(path, newContextContent.ToArray());
            AssetDatabase.Refresh();
        }

        private void CreateRoot()
        {
            var directoryPath = Application.dataPath + CodeGeneratorStrings.RootPath + _rootPath;
            var path = Application.dataPath + CodeGeneratorStrings.RootPath + _rootPath + "/" + _rootName + ".cs";

            var tempRootContent = File.ReadAllLines(CodeGeneratorStrings.TempRootPath);
            var newRootContent = new List<string>();
            
            var namespaceText = "Runtime.Roots." + _rootPath.Replace("/",".");
            
            for (var ii = 0; ii < tempRootContent.Length; ii++)
            {
                var content = tempRootContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceText;
                }
                else if (content.Contains("internal class "))
                {
                    content = "\tpublic class " + _rootName + " : ContextRoot<" + _contextName + ">";
                }

                newRootContent.Add(content);
            }
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            File.WriteAllLines(path, newRootContent.ToArray());
            AssetDatabase.Refresh();
        }
    }
}