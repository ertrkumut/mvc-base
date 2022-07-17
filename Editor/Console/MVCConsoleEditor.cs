using System;
using System.Collections.Generic;
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

        private Dictionary<LogType, bool> _logFilter;
        private Vector2 _logsPanelScroll;
        
        private void OnEnable()
        {
            _logType = ConsoleLogType.All;
            _isCollapsed = true;

            _logFilter = new Dictionary<LogType, bool>
            {
                {LogType.Log, true},
                {LogType.Warning, true},
                {LogType.Error, true}
            };
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
            EditorGUILayout.BeginVertical();
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
            EditorGUILayout.EndHorizontal();

            TopPanelConsoleFilterGUI();
            
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

        private void TopPanelConsoleFilterGUI()
        {
            var logs = MVCConsole.Logs;
            if (_logType != ConsoleLogType.All)
                logs = logs.Where(x => x.consoleLogType == _logType).ToList();
            
            EditorGUILayout.BeginHorizontal();

            _logFilter[LogType.Log] = EditorGUILayout.ToggleLeft("Log(" + logs.Count(x => x.logType == LogType.Log) + ")", _logFilter[LogType.Log], GUILayout.Width(75));
            
            GUI.backgroundColor = Color.yellow;
            _logFilter[LogType.Warning] = EditorGUILayout.ToggleLeft("War(" + logs.Count(x => x.logType == LogType.Warning) + ")", _logFilter[LogType.Warning], GUILayout.Width(75));
            
            GUI.backgroundColor = Color.red;
            _logFilter[LogType.Error] = EditorGUILayout.ToggleLeft("Err(" + logs.Count(x => x.logType == LogType.Error) + ")", _logFilter[LogType.Error], GUILayout.Width(75));
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void LogGUI(ConsoleLog consoleLog)
        {
            if(_logFilter[consoleLog.logType] == false)
                return;
            
            var bgColor = consoleLog.logType == LogType.Log ? Color.white :
                consoleLog.logType == LogType.Warning ? Color.yellow : Color.red;

            GUI.backgroundColor = bgColor;
            
            EditorGUILayout.BeginHorizontal("box");

            var rect = GUILayoutUtility.GetRect(
                new GUIContent(consoleLog.hour.ToString("00") + ":" + consoleLog.minute.ToString("00") + " | " +
                               consoleLog.message), "label");
            GUI.TextField(rect, consoleLog.hour.ToString("00") + ":" + consoleLog.minute.ToString("00") + " | " +
                            consoleLog.message);

            EditorGUILayout.EndHorizontal();
            
            GUI.backgroundColor = Color.white;
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