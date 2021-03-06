using System;
using System.IO;
using System.Linq;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Screen;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateScreenMenu : CreateViewMenu
    {
        protected override string _classLabelName => "Screen Name: ";
        protected override string _classViewName => "ScreenView";
        protected override string _classMediatorName => "ScreenMediator";
        
        protected string _testContextNamespace => "Runtime.Test.Roots.Screens.";

        protected override string _tempViewName => "TempScreenView";
        protected override string _tempMediatorName => "TempScreenMediator";

        protected override string _targetViewPath => CodeGeneratorStrings.ScreenPath;
        protected override string _tempViewPath => CodeGeneratorStrings.TempScreenViewPath;
        protected override string _tempMediatorPath => CodeGeneratorStrings.TempScreenMediatorPath;

        protected override void CreateViewMediator()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.isDirty)
            {
                EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new []{activeScene});
            }
            
            base.CreateViewMediator();

            var rootNamespace = _testContextNamespace + _viewPath.Replace("/", ".");
            var contextName = (_fileName + "TestContext").Replace("View", "");
            var rootName = (_fileName + "TestRoot").Replace("View", "");

            PlayerPrefs.SetString("create-screen-menu-clicked", _fileName);
            PlayerPrefs.SetString("create-screen-root-name", rootName);

            var rootPath = Application.dataPath + CodeGeneratorStrings.TestScreenRootPath + _viewPath;

            CodeGeneratorUtils.CreateContext(contextName, "TempScreenContext", rootPath, CodeGeneratorStrings.TempScreenContextPath,
                rootNamespace);

            CodeGeneratorUtils.CreateRoot(rootName, contextName, "TempScreenContext", "TempScreenRoot", rootPath,
                CodeGeneratorStrings.TempScreenRootPath, rootNamespace);
            
            CodeGeneratorUtils.BindMediationInContext(rootPath + "/" + contextName + ".cs", _viewName, _mediatorName, "TempScreenView", "TempScreenMediator", _viewNamespace);
            CodeGeneratorUtils.ShowScreenInLaunch(rootPath + "/" + contextName + ".cs", _viewName, "TempScreenView", "GameScreens." + _fileName.Replace("View", ""));
            
            CreateScene();
        }

        private void CreateScene()
        {
            var scenePath = CodeGeneratorStrings.ScreenTestScenePath;

            if (!Directory.Exists(scenePath)) 
                Directory.CreateDirectory(scenePath);

            var className = PlayerPrefs.GetString("create-screen-menu-clicked");
            className = className.Replace("View", "");
            var sceneName = className + "TestScene";
            
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
                var path = PlayerPrefs.GetString("create-screen-scene-path") + screenName + ".unity";

                PlayerPrefs.DeleteKey("create-screen-menu-clicked");
                PlayerPrefs.DeleteKey("create-screen-scene-path");
                PlayerPrefs.DeleteKey("create-screen-root-name");
                
                var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
                var currentAssembly = assemblyList.FirstOrDefault(x => x.FullName.StartsWith("Assembly-CSharp,"));
                var screenType = currentAssembly.GetTypes().FirstOrDefault(x => x.Name == screenName);
                var rootType = currentAssembly.GetTypes().FirstOrDefault(x => x.Name == rootName);

                var rootGameObject = new GameObject(rootName).AddComponent(rootType);
                
                var canvasGameObject = new GameObject("Canvas", typeof(Canvas));
                canvasGameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGameObject.transform.SetParent(rootGameObject.transform);
                
                var canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
                canvasScaler.referenceResolution = new Vector2(1080, 1920);
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

                canvasGameObject.AddComponent<GraphicRaycaster>();

                var screenManagerPrefab =
                    AssetDatabase.LoadAssetAtPath<ScreenManager>(CodeGeneratorStrings.ScreenManagerPrefabPath);
                var screenManager =
                    (ScreenManager) PrefabUtility.InstantiatePrefab(screenManagerPrefab, canvasGameObject.transform);

                var screenGameObject = new GameObject(prefabName, typeof(RectTransform));
                screenGameObject.transform.SetParent(screenManager.ScreenLayerList[0].transform);

                var viewInjector = screenGameObject.AddComponent<ViewInjector>();
                screenGameObject.AddComponent(screenType);
                var rectTransform = screenGameObject.transform as RectTransform;
                rectTransform.localScale = Vector3.one;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;

                viewInjector.InitializeForEditor();
                
                if (!Directory.Exists(CodeGeneratorStrings.ScreenPrefabPath))
                    Directory.CreateDirectory(CodeGeneratorStrings.ScreenPrefabPath);
                
                PrefabUtility.SaveAsPrefabAssetAndConnect(screenGameObject, CodeGeneratorStrings.ScreenPrefabPath + prefabName + ".prefab",
                    InteractionMode.UserAction);

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), path);
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