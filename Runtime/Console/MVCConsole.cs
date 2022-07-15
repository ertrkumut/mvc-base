using System;
using System.Collections.Generic;
using MVC.Editor.Console;
using UnityEngine;

namespace MVC.Runtime.Console
{
    internal static class MVCConsole
    {
        public static List<ConsoleLog> Logs = new List<ConsoleLog>();

        public static void Log(ConsoleLogType consoleLogType, string message)
        {
            var log = new ConsoleLog
            {
                hour = DateTime.Now.Hour,
                minute = DateTime.Now.Minute,
                message = message,
                logType = LogType.Log,
                consoleLogType = consoleLogType
            };
            
            Logs.Add(log);
        }
        
        public static void LogWarning(ConsoleLogType consoleLogType, string message)
        {
            var log = new ConsoleLog
            {
                hour = DateTime.Now.Hour,
                minute = DateTime.Now.Minute,
                message = message,
                logType = LogType.Warning,
                consoleLogType = consoleLogType
            };
            
            Logs.Add(log);
        }
        
        public static void LogError(ConsoleLogType consoleLogType, string message)
        {
            var log = new ConsoleLog
            {
                hour = DateTime.Now.Hour,
                minute = DateTime.Now.Minute,
                message = message,
                logType = LogType.Error,
                consoleLogType = consoleLogType
            };
            
            Logs.Add(log);
        }
    }
}