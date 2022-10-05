#if UNITY_EDITOR
using MVC.Editor.CodeGenerator.Menus;
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
        
        [MenuItem("Tools/MVC/Create Screen")]
        private static void CreateScreen()
        {
            EditorWindow.GetWindow<CreateScreenMenu>("Create Screen");
        }

        [MenuItem("Tools/MVC/Create Context")]
        private static void CreateRoot()
        {
            EditorWindow.GetWindow<CreateRootAndContextMenu>("Create Root");
        }
    }
}
#endif