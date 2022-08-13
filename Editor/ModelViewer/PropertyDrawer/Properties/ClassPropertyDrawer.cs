#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MVC.Editor.ModelViewer.PropertyDrawer.Properties
{
    internal class ClassPropertyDrawer<TItem> : PropertyDrawer<TItem>
    {
        private InspectWindow _inspectWindow;

        public ClassPropertyDrawer(string fieldName, bool readOnly) : base(fieldName, readOnly)
        {
        }

        protected override void OnDrawGUI()
        {
            base.OnDrawGUI();

            if (_inspectWindow == null)
            {
                _inspectWindow = ScriptableObject.CreateInstance<InspectWindow>();
                _inspectWindow.Initialize(GetValue(), null);
            }

            EditorGUILayout.BeginVertical("box");
            _inspectWindow.OnGUI();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif