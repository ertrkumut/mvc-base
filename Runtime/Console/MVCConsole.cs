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
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute,
                Second = DateTime.Now.Second,
                Millisecond = DateTime.Now.Millisecond,
                Message = message,
                LogType = LogType.Log,
                ConsoleLogType = consoleLogType
            };
            
            Logs.Add(log);
        }
        
        public static void LogWarning(ConsoleLogType consoleLogType, string message)
        {
            var log = new ConsoleLog
            {
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute,
                Second = DateTime.Now.Second,
                Millisecond = DateTime.Now.Millisecond,
                Message = message,
                LogType = LogType.Warning,
                ConsoleLogType = consoleLogType
            };
            
            Logs.Add(log);
        }
        
        public static void LogError(ConsoleLogType consoleLogType, string message)
        {
            var log = new ConsoleLog
            {
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute,
                Second = DateTime.Now.Second,
                Millisecond = DateTime.Now.Millisecond,
                Message = message,
                LogType = LogType.Error,
                ConsoleLogType = consoleLogType
            };
            
            Logs.Add(log);
        }
    }
}