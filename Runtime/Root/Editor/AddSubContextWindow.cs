#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.CodeGenerator;
using MVC.Runtime.Contexts;
using MVC.Runtime.Root;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MVC.Root.Editor
{
    public class AddSubContextWindow : EditorWindow
    {
        private string searchText = "";
        private Vector2 scrollPosition;
        
        private RootBase _root;
        private List<Type> _contextTypeList;
        
        public static void ShowWindow(RootBase root)
        {
            var window = GetWindow<AddSubContextWindow>("Add Sub Context");
            window._root = root;
            window.LoadContextTypes();
        }
        
        private void LoadContextTypes()
        {
            var types = AssemblyHelper.GetAllTypesFromAssemblies();
            _contextTypeList = types
                .Where(x => typeof(IContext).IsAssignableFrom(x))
                .Where(x => _root.SubContextTypes.FirstOrDefault(a => a.ContextFullName == x.FullName).Equals(default))
                .Where(x => x.GetField(CodeGeneratorStrings.ContextTestFlag, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic) == null)
                .ToList();
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Search Context:", EditorStyles.boldLabel);
            var newSearchText = GUILayout.TextField(searchText, "ToolbarSearchTextField");
            if (GUILayout.Button("", searchText != "" ? "ToolbarSearchCancelButton" : "ToolbarSearchCancelButtonEmpty"))
            {
                // Clear the search text when the cancel button is pressed
                newSearchText = "";
                GUI.FocusControl(null);
            }
            
            if (newSearchText != null && newSearchText != searchText)
            {
                searchText = newSearchText;
                
            }
            IEnumerable<Type> filteredList = _contextTypeList;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredList = _contextTypeList
                    .Where(x => x.Name.ToLowerInvariant().Contains(searchText.ToLowerInvariant()));
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (var type in filteredList)
            {
                if (GUILayout.Button(type.Name))
                {
                    _root.SubContextTypes.Add(new SubContextData
                    {
                        ContextFullName = type.FullName,
                        ContextName = type.Name
                    });

                    MarkDirty();
                    Close(); // Close the window after selection
                }
            }

            GUILayout.EndScrollView();
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