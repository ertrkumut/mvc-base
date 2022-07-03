namespace MVC.Runtime.Controller
{
    public interface ICommandBody
    {
        bool IsRetain { get; set; }

        void Retain();
        void Release(params object[] sequenceData);

        void Jump<TCommandType>(params object[] sequenceData)
            where TCommandType : ICommandBody;
        
        void Stop();

        void Clean();
    }
}