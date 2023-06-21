using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Attributes;
using UnityEngine;

namespace MVC.Runtime.Controller
{
    public class CommandBody : ICommandBody
    {
        [Inject] protected ICommandBinder CommandBinder { get; set; }
        
        public bool IsRetain { get; set; }
        public bool HasRetain { get; set; }

        public virtual void Retain()
        {
            IsRetain = true;
            HasRetain = true;
        }

        public virtual void Release(params object[] sequenceData)
        {
            CommandBinder.ReleaseCommand(this, sequenceData);
        }
        
        public virtual void Jump<TCommandType>(params object[] sequenceData)
            where TCommandType : ICommandBody
        {
            if (!IsRetain)
            {
                Debug.LogError("Command must be retain, if you want to JUMP!");
                MVCConsole.LogError(ConsoleLogType.Command, "Command must be retain, if you want to JUMP!");
                return;
            }
            
            CommandBinder.Jump<TCommandType>(this, sequenceData);
        }

        public virtual void Stop()
        {
            CommandBinder.StopCommand(this);
        }

        public virtual void Clean()
        {
            IsRetain = false;
        }
    }
}