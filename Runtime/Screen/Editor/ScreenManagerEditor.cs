#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MVC.Runtime.Screen
{
    [CustomEditor(typeof(ScreenManager), true)]
    [CanEditMultipleObjects]
    public class ScreenManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if(!Application.isPlaying)
            {
                var allScreenManagers = FindObjectsOfType<ScreenManager>().ToList();
                foreach (var screenManager in allScreenManagers)
                {
                    var managerIndex = screenManager.ManagerIndex;
                    var sameIndexManagers = allScreenManagers
                        .Where(x => x.ManagerIndex == managerIndex)
                        .ToList();

                    if (sameIndexManagers.Count != 1)
                    {
                        EditorGUILayout.HelpBox("There is too many ScreenManagers with same Index!!",
                            MessageType.Error);
                        break;
                    }
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}
#endif