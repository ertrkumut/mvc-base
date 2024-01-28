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

namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateScreenMenu : CreateViewMenu
    {
        private const string KEY_FILE_NAME = "file-name";
        private const string KEY_ROOT_NAME = "root-name";
        private const string KEY_PARENT_FOLDER_PATH = "parent-folder-path";
        private const string KEY_VIEW_NAMESPACE = "view-namespace";
        private const string KEY_CONTEXT_NAMESPACE = "context-namespace";
        private const string KEY_SCREEN_NAME = "screen-scene-name";
        private const string KEY_SCENE_PATH = "scene-path";
        private const string KEY_CREATE_SCENE = "create-scene";
        
        protected override bool _showCreateSceneToggle => true;

        protected override string _classLabelName => "Screen Name: ";
        protected override string _classViewName => "ScreenView";
        protected override string _classMediatorName => "ScreenMediator";

        protected override string _tempViewName => "TempScreenView";
        protected override string _tempMediatorName => "TempScreenMediator";

        protected override string _targetViewPath => CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenViewPath, _parentFolderName);
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

            PlayerPrefs.SetString(KEY_FILE_NAME, _fileName);
            PlayerPrefs.SetString(KEY_ROOT_NAME, rootName);
            PlayerPrefs.SetString(KEY_PARENT_FOLDER_PATH, _parentFolderName);
            PlayerPrefs.SetString(KEY_VIEW_NAMESPACE, _viewNamespace);
            PlayerPrefs.SetString(KEY_CONTEXT_NAMESPACE, contextNamespace);
            PlayerPrefs.SetInt(KEY_CREATE_SCENE, _createScene ? 1 : 0);

            CodeGeneratorUtils.CreateContext(contextName, "TempScreenTestContext", contextPath, CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenTestContextPath, _parentFolderName),
                contextNamespace, true,true);

            CodeGeneratorUtils.CreateRoot(rootName, contextName, "TempScreenTestContext", "TempScreenTestRoot", contextPath,
                CodeGeneratorStrings.GetPath(CodeGeneratorStrings.TempScreenTestRootPath, _parentFolderName), contextNamespace, true);
            
            CodeGeneratorUtils.BindMediationInContext(contextPath + "/" + contextName + ".cs", _viewName, _mediatorName, "TempScreenView", "TempScreenMediator", _viewNamespace);
            CodeGeneratorUtils.ShowScreenInLaunch(contextPath + "/" + contextName + ".cs", _viewName, "TempScreenView", "GameScreens." + _fileName.Replace("View", ""), _parentFolderName);
            
            CreateScene();
        }

        private void CreateScene()
        {
            var scenePath = CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenTestScenePath, _parentFolderName);

            if (!Directory.Exists(scenePath)) 
                Directory.CreateDirectory(scenePath);

            var className = PlayerPrefs.GetString(KEY_FILE_NAME);
            className = className.Replace("View", "");
            var sceneName = className + "TestScene";
            
            PlayerPrefs.SetString(KEY_SCREEN_NAME, sceneName);
            
            CodeGeneratorUtils.CreateScreenEnum(_parentFolderName, className);
            
            if(!_createScene)
                return;
            
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            scene.name = sceneName;

            PlayerPrefs.SetString(KEY_SCENE_PATH, scenePath);
        }

        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CodeGenerationCompleted()
        {
            try
            {
                if (!PlayerPrefs.HasKey(KEY_FILE_NAME)) 
                    return;

                var screenName = PlayerPrefs.GetString(KEY_FILE_NAME);
                var screenNamespace = PlayerPrefs.GetString(KEY_VIEW_NAMESPACE);
                var rootName = PlayerPrefs.GetString(KEY_ROOT_NAME);
                var rootNamespace = PlayerPrefs.GetString(KEY_CONTEXT_NAMESPACE);

                var prefabName = screenName.Replace("View", "");
                var sceneName = PlayerPrefs.GetString(KEY_SCREEN_NAME);
                var path = PlayerPrefs.GetString(KEY_SCENE_PATH) + sceneName + ".unity";
                var parentFolderName = PlayerPrefs.GetString(KEY_PARENT_FOLDER_PATH);

                var createScene = PlayerPrefs.GetInt(KEY_CREATE_SCENE) == 1;
                
                PlayerPrefs.DeleteKey(KEY_FILE_NAME);
                PlayerPrefs.DeleteKey(KEY_SCENE_PATH);
                PlayerPrefs.DeleteKey(KEY_ROOT_NAME);
                PlayerPrefs.DeleteKey(KEY_PARENT_FOLDER_PATH);
                PlayerPrefs.DeleteKey(KEY_VIEW_NAMESPACE);
                PlayerPrefs.DeleteKey(KEY_CONTEXT_NAMESPACE);
                PlayerPrefs.DeleteKey(KEY_CREATE_SCENE);

                if (!createScene)
                    return;
                
                var possibleAssemblyFiles = AssemblyHelper.GetAllTypesFromAssemblies();

                var screenType = possibleAssemblyFiles.FirstOrDefault(x => x.Name == screenName && x.Namespace == screenNamespace);;
                var rootType = possibleAssemblyFiles.FirstOrDefault(x => x.Name == rootName && x.Namespace == rootNamespace);;

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
                PlayerPrefs.DeleteKey(KEY_FILE_NAME);
                PlayerPrefs.DeleteKey(KEY_SCENE_PATH);
                PlayerPrefs.DeleteKey(KEY_ROOT_NAME);
                PlayerPrefs.DeleteKey(KEY_PARENT_FOLDER_PATH);
                PlayerPrefs.DeleteKey(KEY_VIEW_NAMESPACE);
                PlayerPrefs.DeleteKey(KEY_CONTEXT_NAMESPACE);
                PlayerPrefs.DeleteKey(KEY_CREATE_SCENE);
                
                Debug.LogError(e);
            }
        }
    }
}
#endif