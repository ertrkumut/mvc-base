#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace MVC.Editor.CodeGenerator
{
    [CreateAssetMenu(fileName = "MVCCodeGenerationSettings", menuName = "MVC/CodeGeneration/Settings")]
    public class MVCCodeGenerationSettings : ScriptableObject
    {
        public List<string> ParentFolderNames = new();
        public string MainFolderName = "Runtime";

        public List<AssemblyDefinitionAsset> AssemblyDefinitions;
    }
}
#endif