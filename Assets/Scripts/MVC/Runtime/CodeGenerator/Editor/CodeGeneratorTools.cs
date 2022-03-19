using MVC.Runtime.CodeGenerator.Editor.Menus;
using UnityEditor;

namespace MVC.Runtime.CodeGenerator.Editor
{
    internal static class CodeGeneratorTools
    {
        [MenuItem("Tools/MVC/Create View")]
        private static void CreateView()
        {
            EditorWindow.GetWindow<CreateViewMenu>();
        }
    }
}