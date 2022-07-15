using System;
using System.Linq;
using MVC.Runtime.Console;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.Console
{
    internal class MVCConsoleEditor : EditorWindow
    {
        [MenuItem("Tools/MVC/MVC Console", false, 1)]
        private static void OpenConsole()
        {
            var window = GetWindow<MVCConsoleEditor>("System Console");
            window.position = new Rect(window.position.position, new Vector2(600, 300));
        }

//======================================================================================================================
//  ===========   Properties   ===========

        private Action OnConsoleLogTypeChanged { get; set; }

        private ConsoleLogType _logType;
        private bool _isCollapsed;

        private Vector2 _logsPanelScroll;
        
        private void OnEnable()
        {
            _logType = ConsoleLogType.All;
            _isCollapsed = true;
        }

        private void OnDisable()
        {
            
        }

        private void OnGUI()
        {
            TopPanelGUI();
            LogsPanelGUI();   
        }

        private void Update()
        {
            if(!Application.isPlaying)
                return;
            
            Repaint();
        }

        private void TopPanelGUI()
        {
            EditorGUILayout.BeginHorizontal();

            var consoleLogTypeArray = Enum.GetValues(typeof(ConsoleLogType));
            for (var ii = 0; ii < consoleLogTypeArray.Length; ii++)
            {
                var consoleLogType = (ConsoleLogType) consoleLogTypeArray.GetValue(ii);

                GUI.backgroundColor = consoleLogType == _logType ? Color.green : Color.white;
                var button = GUILayout.Button(consoleLogType.ToString());
                GUI.backgroundColor = Color.white;

                if (button)
                {
                    ChangeConsoleLogType(consoleLogType);
                }
            }

            var collapseButtonStyle = EditorStyles.miniButton;

            GUI.backgroundColor = _isCollapsed ? Color.gray : Color.white;
            if (GUILayout.Button("Collapse", collapseButtonStyle, GUILayout.Width(100)))
            {
                _isCollapsed = !_isCollapsed;
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void LogsPanelGUI()
        {
            var logs = MVCConsole.Logs;

            if (_logType != ConsoleLogType.All)
            {
                logs = logs.Where(x => x.consoleLogType == _logType).ToList();
            }
            
            _logsPanelScroll = EditorGUILayout.BeginScrollView(_logsPanelScroll);

            for (var ii = 0; ii < logs.Count; ii++)
            {
                LogGUI(logs[ii]);
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void LogGUI(ConsoleLog consoleLog)
        {
            EditorGUILayout.BeginHorizontal("box");
            
            EditorGUILayout.LabelField(consoleLog.hour.ToString("00") + ":" + consoleLog.minute.ToString("00") + " | " + consoleLog.message);

            EditorGUILayout.EndHorizontal();
        }

        private void ChangeConsoleLogType(ConsoleLogType consoleLogType)
        {
            if(_logType.Equals(consoleLogType))
                return;
            
            _logType = consoleLogType;
            OnConsoleLogTypeChanged?.Invoke();
        }
    }
}