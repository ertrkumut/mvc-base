using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Attributes;

namespace MVC.Runtime.Controller
{
    public abstract class Command : ICommandBody
    {
        [Inject] protected CommandBinder CommandBinder { get; set; }

        public abstract void Execute();
        
        public bool Retain { get; set; }

        public int SequenceId { get; set; }
        
        public virtual void RetainCommand()
        {
            Retain = true;
        }

        public virtual void ReleaseCommand(params object[] sequenceData)
        {
            // _commandSequencer.ReleaseCommand(this);
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