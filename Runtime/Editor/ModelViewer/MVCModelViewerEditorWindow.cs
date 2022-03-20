using MVC.Runtime.Attributes;
using MVC.Runtime.Injectable;
using MVC.Runtime.Root;
using UnityEditor;
using UnityEngine;

namespace MVC.Runtime.Editor.ModelViewer
{
    internal class MVCModelViewerEditorWindow : EditorWindow
    {
        [MenuItem("Tools/MVC/Model Viewer")]
        private static void OpenModelViewer()
        {
            GetWindow<MVCModelViewerEditorWindow>("Model Viewer");
        }
        
        private RootBase _inspectedRoot;

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("Enter Play Mode to view Models.");
                return;
            }
            
            if (_inspectedRoot == null)
            {
                DisplayAllRootsAndSelectGUI();
                return;
            }

            DisplayAllInjectedTypesGUI();
        }

        private void DisplayAllRootsAndSelectGUI()
        {
            var rootObjects = FindObjectsOfType<RootBase>();
            EditorGUILayout.BeginVertical("box");
            
            foreach (var contextRoot in rootObjects)
            {
                if (GUILayout.Button(contextRoot.name))
                {
                    _inspectedRoot = contextRoot;
                }
            }
                
            EditorGUILayout.EndVertical();
        }
        
        private void DisplayAllInjectedTypesGUI()
        {
            GUI.backgroundColor = Color.red;
            var backButton = GUILayout.Button("Select Context");
            GUI.backgroundColor = Color.white;
            
            if (backButton)
            {
                _inspectedRoot = null;
                return;
            }

            EditorGUILayout.Space(15);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Local Injected Objects", EditorStyles.boldLabel);
            
            var injectionBinder = _inspectedRoot.Context.InjectionBinder;
            var injectedObjects = injectionBinder.GetInjectedInstances();

            foreach (InjectionBinding injectedObject in injectedObjects)
            {
                DrawInjectedObject(injectedObject);
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Cross Context Injected Objects", EditorStyles.boldLabel);
            
            var crossContextInjectionBinder = _inspectedRoot.Context.CrossContextInjectionBinder;
            var crossContextInjectedObjects = crossContextInjectionBinder.GetInjectedInstances();

            foreach (InjectionBinding injectedObject in crossContextInjectedObjects)
            {
                DrawInjectedObject(injectedObject);
            }
            
            EditorGUILayout.EndVertical();
        }

        private void DrawInjectedObject(InjectionBinding injectedObject)
        {
            var hideAttribute = injectedObject.Value.GetType().GetCustomAttributes(typeof(HideFromModelViewerAttribute), true).Length != 0;
            if(hideAttribute)
                return;

            var injectionName = injectedObject.Name != "" ? " - {" + injectedObject.Name + "}" : "";
            
            if (GUILayout.Button(injectedObject.Value.GetType().Name + injectionName))
            {
                var window = CreateWindow<MVCInspectModelWindow>();
                window.Initialize(injectedObject.Value, _inspectedRoot.Context);
            }
        }
    }
}