using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Attributes;

namespace MVC.Runtime.Controller
{
    public class CommandBody : ICommandBody
    {
        [Inject] protected ICommandBinder CommandBinder { get; set; }
        
        public bool Retain { get; set; }

        public int SequenceId { get; set; }
        
        public virtual void RetainCommand()
        {
            Retain = true;
        }

        public virtual void ReleaseCommand(params object[] sequenceData)
        {
            CommandBinder.ReleaseCommand(this, sequenceData);
        }

        public virtual void FailCommand()
        {
        }

        public virtual void Clean()
        {
            Retain = false;
        }
    }
}