namespace MVC.Runtime.Controller
{
    public interface ICommandBody
    {
        bool IsRetain { get; set; }

        void Retain();
        void Release(params object[] sequenceData);
        void Stop();

        void Clean();
    }
}