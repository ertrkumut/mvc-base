#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Screen;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateScreenMenu : CreateViewMenu
    {
        protected override string _classLabelName => "Screen Name: ";
        protected override string _classViewName => "ScreenView";
        protected override string _classMediatorName => "ScreenMediator";

        protected override string _tempViewName => "TempScreenView";
        protected override string _tempMediatorName => "TempScreenMediator";

        protected override string _targetViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenPath, _parentFolderName);
        protected override string _tempViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenViewPath, _parentFolderName);
        protected override string _tempMediatorPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenMediatorPath, _parentFolderName);

        protected override void GUI_ContextList()
        {
            DrawAllContexts(true);
        }

        protected override void CreateViewMediator()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.isDirty)
            {
                EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new []{activeScene});
            }
            
            base.CreateViewMediator();

            var contextName = (_fileName + "TestContext").Replace("View", "");
            var rootName = (_fileName + "TestRoot").Replace("View", "");
            var contextPath = Application.dataPath + string.Format(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TestScreenRootPath, _parentFolderName), contextName) + _viewNameInputField;
            
            var contextNamespace = contextPath
                .Replace(Application.dataPath + "/Test/Scripts/", "")
                .Replace("/", ".")
                .TrimEnd('.');
            contextNamespace = "Test." + contextNamespace;

            PlayerPrefs.SetString("create-screen-menu-clicked", _fileName);
            PlayerPrefs.SetString("create-screen-root-name", rootName);
            PlayerPrefs.SetString("parent-folder-path", _parentFolderName);

            CodeGeneratorUtils.CreateContext(contextName, "TempScreenTestContext", contextPath, CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenTestContextPath, _parentFolderName),
                contextNamespace, true,true);

            CodeGeneratorUtils.CreateRoot(rootName, contextName, "TempScreenTestContext", "TempScreenTestRoot", contextPath,
                CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenTestRootPath, _parentFolderName), contextNamespace, true);
            
            CodeGeneratorUtils.BindMediationInContext(contextPath + "/" + contextName + ".cs", _viewName, _mediatorName, "TempScreenView", "TempScreenMediator", _viewNamespace);
            CodeGeneratorUtils.ShowScreenInLaunch(contextPath + "/" + contextName + ".cs", _viewName, "TempScreenView", "GameScreens." + _fileName.Replace("View", ""));
            
            CreateScene();
        }

        private void CreateScene()
        {
            var scenePath = CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenTestScenePath, _parentFolderName);

            if (!Directory.Exists(scenePath)) 
                Directory.CreateDirectory(scenePath);

            var className = PlayerPrefs.GetString("create-screen-menu-clicked");
            className = className.Replace("View", "");
            var sceneName = className + "TestScene";
            
            PlayerPrefs.SetString("screen-scene-name", sceneName);
            
            CodeGeneratorUtils.CreateScreenEnum(className);
            
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            scene.name = sceneName;

            PlayerPrefs.SetString("create-screen-scene-path", scenePath);
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CodeGenerationCompleted()
        {
            try
            {
                if (!PlayerPrefs.HasKey("create-screen-menu-clicked")) 
                    return;

                var screenName = PlayerPrefs.GetString("create-screen-menu-clicked");
                var rootName = PlayerPrefs.GetString("create-screen-root-name");

                var prefabName = screenName.Replace("View", "");
                var sceneName = PlayerPrefs.GetString("screen-scene-name");
                var path = PlayerPrefs.GetString("create-screen-scene-path") + sceneName + ".unity";
                var parentFolderName = PlayerPrefs.GetString("parent-folder-path");
                
                PlayerPrefs.DeleteKey("create-screen-menu-clicked");
                PlayerPrefs.DeleteKey("create-screen-scene-path");
                PlayerPrefs.DeleteKey("create-screen-root-name");
                PlayerPrefs.DeleteKey("parent-folder-path");
                
                var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
                var currentAssembly = assemblyList.FirstOrDefault(x => x.FullName.StartsWith("Assembly-CSharp,"));
                var screenType = currentAssembly.GetTypes().FirstOrDefault(x => x.Name == screenName);
                var rootType = currentAssembly.GetTypes().FirstOrDefault(x => x.Name == rootName);

                var rootGameObject = new GameObject(rootName).AddComponent(rootType);

                var screenManagerPrefab =
                    AssetDatabase.LoadAssetAtPath<ScreenManager>(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenManagerPrefabPath, parentFolderName));
                var screenManager =
                    (ScreenManager) PrefabUtility.InstantiatePrefab(screenManagerPrefab, rootGameObject.transform);

                var screenGameObject = new GameObject(prefabName, typeof(RectTransform));
                screenGameObject.transform.SetParent(screenManager.ScreenLayerList[0].transform);

                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
                
                var viewInjector = screenGameObject.AddComponent<ViewInjector>();
                screenGameObject.AddComponent(screenType);
                var rectTransform = screenGameObject.transform as RectTransform;
                rectTransform.localScale = Vector3.one;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;

                viewInjector.InitializeForEditor();
                
                if (!Directory.Exists(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenPrefabPath, parentFolderName)))
                    Directory.CreateDirectory(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenPrefabPath, parentFolderName));
                
                PrefabUtility.SaveAsPrefabAssetAndConnect(screenGameObject, CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenPrefabPath, parentFolderName) + prefabName + ".prefab",
                    InteractionMode.UserAction);

                var relativeScenePath = path.Replace(Application.dataPath, "");
                relativeScenePath = "Assets/" + relativeScenePath;
                
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), relativeScenePath);
                AssetDatabase.Refresh();

                Selection.activeGameObject = screenGameObject;
            }
            catch (Exception e)
            {
                PlayerPrefs.DeleteKey("create-screen-menu-clicked");
                PlayerPrefs.DeleteKey("create-screen-scene-path");
                PlayerPrefs.DeleteKey("create-screen-root-name");
                
                Debug.LogError(e);
            }
        }
    }
}
#endif