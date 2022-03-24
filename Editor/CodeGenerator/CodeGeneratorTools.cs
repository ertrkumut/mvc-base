﻿using MVC.Editor.CodeGenerator.Menus;
using UnityEditor;

namespace MVC.Editor.CodeGenerator
{
    internal static class CodeGeneratorTools
    {
        [MenuItem("Tools/MVC/Create View")]
        private static void CreateView()
        {
            EditorWindow.GetWindow<CreateViewMenu>("Create View");
        }

        [MenuItem("Tools/MVC/Create Root")]
        private static void CreateRoot()
        {
            EditorWindow.GetWindow<CreateRootMenu>("Create Root");
        }
    }
}