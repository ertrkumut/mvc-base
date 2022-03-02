namespace MVC.Runtime.Controller
{
    public interface ICommandBody
    {
        bool Retain { get; set; }

        int SequenceId { get; set; }

        void RetainCommand();
        void ReleaseCommand(params object[] sequenceData);
        void FailCommand();

        void Clean();
    }
}