#if UNITY_EDITOR
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

        private ConsoleLogType _selectedLogType;

        private Dictionary<LogType, bool> _logFilter;
        private Vector2 _logsPanelScroll;

        private Dictionary<ConsoleLogType, List<ConsoleLog>> _logDictionary;

        private void OnEnable()
        {
            _selectedLogType = ConsoleLogType.All;

            _logFilter = new Dictionary<LogType, bool>
            {
                {LogType.Log, true},
                {LogType.Warning, true},
                {LogType.Error, true}
            };

            InitializeLogs();
            
            MVCConsole.OnLogAdded += OnLogAdded;
        }

        private void OnDisable()
        {
            MVCConsole.OnLogAdded -= OnLogAdded;
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

        private void InitializeLogs()
        {
            _logDictionary = new Dictionary<ConsoleLogType, List<ConsoleLog>>();

            var logs = MVCConsole.Logs;

            var consoleLogTypeArray = Enum.GetValues(typeof(ConsoleLogType));
            for (var ii = 0; ii < consoleLogTypeArray.Length; ii++)
            {
                var consoleLogType = (ConsoleLogType) consoleLogTypeArray.GetValue(ii);
                _logDictionary.Add(consoleLogType, new List<ConsoleLog>());
            }

            foreach (var consoleLog in logs)
            {
                _logDictionary[ConsoleLogType.All].Add(consoleLog);
                _logDictionary[consoleLog.ConsoleLogType].Add(consoleLog);
            }
        }
        
        private void TopPanelGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            var consoleLogTypeArray = Enum.GetValues(typeof(ConsoleLogType));
            for (var ii = 0; ii < consoleLogTypeArray.Length; ii++)
            {
                var consoleLogType = (ConsoleLogType) consoleLogTypeArray.GetValue(ii);

                GUI.backgroundColor = consoleLogType == _selectedLogType ? Color.green : Color.white;
                var button = GUILayout.Button(consoleLogType.ToString());
                GUI.backgroundColor = Color.white;

                if (button)
                {
                    ChangeConsoleLogType(consoleLogType);
                }
            }

            EditorGUILayout.EndHorizontal();

            TopPanelConsoleFilterGUI();
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void LogsPanelGUI()
        {
            _logsPanelScroll = EditorGUILayout.BeginScrollView(_logsPanelScroll);

            var logs = _logDictionary[_selectedLogType];
            for (var ii = 0; ii < logs.Count; ii++)
            {
                LogGUI(logs[ii]);
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void TopPanelConsoleFilterGUI()
        {
            var logs = _logDictionary[_selectedLogType];;

            EditorGUILayout.BeginHorizontal();

            _logFilter[LogType.Log] = EditorGUILayout.ToggleLeft("Log(" + logs.Count(x => x.LogType == LogType.Log) + ")", _logFilter[LogType.Log], GUILayout.Width(75));
            
            GUI.backgroundColor = Color.yellow;
            _logFilter[LogType.Warning] = EditorGUILayout.ToggleLeft("War(" + logs.Count(x => x.LogType == LogType.Warning) + ")", _logFilter[LogType.Warning], GUILayout.Width(75));
            
            GUI.backgroundColor = Color.red;
            _logFilter[LogType.Error] = EditorGUILayout.ToggleLeft("Err(" + logs.Count(x => x.LogType == LogType.Error) + ")", _logFilter[LogType.Error], GUILayout.Width(75));
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void LogGUI(ConsoleLog consoleLog)
        {
            if(_logFilter[consoleLog.LogType] == false)
                return;
            
            var bgColor = consoleLog.LogType == LogType.Log ? Color.white :
                consoleLog.LogType == LogType.Warning ? Color.yellow : Color.red;

            GUI.backgroundColor = bgColor;
            
            EditorGUILayout.BeginHorizontal("box");

            var date = consoleLog.Hour.ToString("00") + ":" + consoleLog.Minute.ToString("00") + ":" + consoleLog.Second.ToString("00") + ":" + consoleLog.Millisecond.ToString("0000") + " | ";
            var rect = GUILayoutUtility.GetRect(new GUIContent(date + " | " + consoleLog.Message), "label");
            GUI.TextField(rect, date + " | " + consoleLog.Message);

            EditorGUILayout.EndHorizontal();
            
            GUI.backgroundColor = Color.white;
        }

        private void ChangeConsoleLogType(ConsoleLogType consoleLogType)
        {
            if(_selectedLogType.Equals(consoleLogType))
                return;
            
            _selectedLogType = consoleLogType;
            OnConsoleLogTypeChanged?.Invoke();
        }
        
        private void OnLogAdded(ConsoleLog log)
        {
            _logDictionary[ConsoleLogType.All].Add(log);
            
            _logDictionary[log.ConsoleLogType].Add(log);
        }
    }
}
#endif