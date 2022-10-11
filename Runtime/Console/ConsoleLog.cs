using MVC.Editor.Console;
using UnityEngine;

namespace MVC.Runtime.Console
{
    internal class ConsoleLog
    {
        public int Hour;
        public int Minute;
        public int Second;
        public int Millisecond;

        public ConsoleLogType ConsoleLogType;
        public LogType LogType;
        public string Message;
    }
}