using MVC.Runtime.CodeGenerator.Editor.Menus;
using UnityEditor;

namespace MVC.Runtime.CodeGenerator.Editor
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