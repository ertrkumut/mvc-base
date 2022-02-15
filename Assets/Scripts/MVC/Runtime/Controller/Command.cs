namespace MVC.Runtime.Controller
{
    public abstract class Command : ICommandBody
    {
        public abstract void Execute();
        
        public bool Retain { get; set; }
        public int SequenceId { get; set; }
        
        public virtual void RetainCommand()
        {
            Retain = true;
        }

        public virtual void ReleaseCommand(params object[] sequenceData)
        {
            Retain = false;
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