using UnityEngine;

namespace MVC.Editor.CodeGenerator
{
    [CreateAssetMenu(fileName = "MVCCodeGenerationSettings", menuName = "MVC/CodeGeneration/Settings")]
    public class MVCCodeGenerationSettings : ScriptableObject
    {
        public string MainFolderName = "Runtime";
    }
}