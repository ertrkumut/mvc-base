using UnityEngine;

namespace MVC.Runtime.CodeGenerator.Editor
{
    internal static class CodeGeneratorStrings
    {
        internal const string ViewPath = "/Scripts/Runtime/Views/";
        internal const string RootPath = "/Scripts/Runtime/Roots/";

        internal static readonly string TempViewPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Runtime/CodeGenerator/Editor/TempViews/TempView.cs";
        internal static readonly string TempMediatorPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Runtime/CodeGenerator/Editor/TempViews/TempMediator.cs";

        internal static readonly string TempContextPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Runtime/CodeGenerator/Editor/TempRoots/TempContext.cs";
        internal static readonly string TempRootPath = Application.dataPath.Replace("Assets", "") + "Packages/unity-mvc/Runtime/CodeGenerator/Editor/TempRoots/TempRoot.cs";
    }
}