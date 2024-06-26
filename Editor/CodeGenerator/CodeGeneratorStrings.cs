#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.CodeGenerator
{
    internal static class CodeGeneratorStrings
    {
        internal const string ViewPath = "/Scripts/$/Contexts/{0}/Views/";
        internal const string TestViewPath = "/Test/Scripts/Contexts/{0}/Views/";
        internal const string ScreenViewPath = "/Scripts/$/Contexts/{0}/Views/Screens/";
        internal const string ContextPath = "/Scripts/$/Contexts/{0}/Root/";
        internal const string TestContextPath = "/Test/Scripts/Contexts/{0}/Root/";

        internal static readonly string TempViewPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempViews/TempView.cs";
        internal static readonly string TempMediatorPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempViews/TempMediator.cs";

        internal static readonly string TempContextPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempRoots/TempContext.cs";
        internal static readonly string TempRootPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempRoots/TempRoot.cs";
        
        internal static readonly string ContextScenePath = Application.dataPath + "/Scenes/";
        internal static readonly string TestContextScenePath = Application.dataPath + "/Test/Scenes/";

        internal static readonly string ScreenManagerPrefabPath = "Packages/mvc-base/Assets/Resources/Screen/ScreenManager.prefab";
        internal static readonly string ScreenPrefabPath = Application.dataPath + "/Resources/Screens/";
        internal static readonly string ScreenTestScenePath = Application.dataPath + "/Test/Scenes/Screens/";
        internal static readonly string TempScreenViewPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempScreens/TempScreenView.cs";
        internal static readonly string TempScreenMediatorPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempScreens/TempScreenMediator.cs";
        
        internal static readonly string TempScreenContextPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempScreens/TempScreenContext.cs";
        internal static readonly string TempScreenRootPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempScreens/TempScreenRoot.cs";
        
        internal static readonly string TempScreenTestContextPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempScreens/TempScreenTestContext.cs";
        internal static readonly string TempScreenTestRootPath = Application.dataPath.Replace("Assets", "") + "Packages/mvc-base/Editor/CodeGenerator/TempScreens/TempScreenTestRoot.cs";
        
        internal static readonly string ScreenTypeEnumPath = Application.dataPath + "/Scripts/$/Contexts/Screen/Enums/";
        internal static readonly string ScreenTypeEnumFileName = "GameScreens";

        internal static readonly string TestScreenRootPath = "/Test/Scripts/Contexts/Screens/{0}/Root/";

        internal const string ContextScreenFlag = "FLAG_screenContext";
        internal const string ContextTestFlag = "FLAG_testContext";


        internal static string GetPath(string path, string parentFolderName)
        {
            return string.IsNullOrEmpty(parentFolderName) 
                ? path.Replace("$", "Runtime") 
                : path.Replace("$", parentFolderName);
        }
        internal static MVCCodeGenerationSettings GetCodeGenerationSettings()
        {
            //return Resources.Load<MVCCodeGenerationSettings>("MVCCodeGenerationSettings");
            return AssetDatabase.LoadAssetAtPath<MVCCodeGenerationSettings>("Assets/Editor/MVCCodeGenerationSettings.asset");
        }
    }
}
#endif