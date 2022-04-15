using UnityEngine;

namespace MVC.Editor.CodeGenerator
{
    internal static class CodeGeneratorStrings
    {
        internal const string ViewPath = "/Scripts/Runtime/Views/";
        internal const string ScreenPath = "/Scripts/Runtime/Views/Screens/";
        internal const string RootPath = "/Scripts/Runtime/Roots/";

        internal static readonly string TempViewPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Editor/CodeGenerator/TempViews/TempView.cs";
        internal static readonly string TempMediatorPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Editor/CodeGenerator/TempViews/TempMediator.cs";

        internal static readonly string TempContextPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Editor/CodeGenerator/TempRoots/TempContext.cs";
        internal static readonly string TempRootPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Editor/CodeGenerator/TempRoots/TempRoot.cs";
        internal static readonly string RootScenePath = Application.dataPath + "/Scenes/";

        internal static readonly string ScreenPrefabPath = Application.dataPath + "/Resources/Screens/";
        internal static readonly string ScreenTestScenePath = Application.dataPath + "/Test/Screens/";
        internal static readonly string TempScreenViewPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Editor/CodeGenerator/TempScreens/TempScreenView.cs";
        internal static readonly string TempScreenMediatorPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Editor/CodeGenerator/TempScreens/TempScreenMediator.cs";
    }
}