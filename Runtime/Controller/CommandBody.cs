using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Attributes;
using UnityEngine;

namespace MVC.Runtime.Controller
{
    public class CommandBody : ICommandBody
    {
        [Inject] protected ICommandBinder CommandBinder { get; set; }
        
        public bool IsRetain { get; set; }

        public virtual void Retain()
        {
            IsRetain = true;
        }

        public virtual void Release(params object[] sequenceData)
        {
            if (!IsRetain)
            {
                Debug.LogError("Command must be retain, if you want to call manual RELEASE!");
                return;
            }
            CommandBinder.ReleaseCommand(this, sequenceData);
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