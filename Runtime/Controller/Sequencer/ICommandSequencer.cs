using System;
using MVC.Runtime.Controller.Binder;

namespace MVC.Runtime.Controller.Sequencer
{
    internal interface ICommandSequencer
    {
        Action<ICommandSequencer> SequenceFinished { get; set; }
        
        ICommandBody CurrentCommand { get; set; }

        void Initialize(ICommandBinding commandBinding, CommandBinder commandBinder, params object[] signalParameters);

        void RunCommands();

        internal void ExecuteCommand(params object[] commandParameters);
        internal void ParallelAutoReleaseCommand(ICommandBody command);
        internal void SequenceAutoReleaseCommand(ICommandBody command);

        void ReleaseCommand(ICommandBody command, params object[] commandParameters);

        void JumpCommand<TCommandType>(ICommandBody command, params object[] commandParameters)
            where TCommandType : ICommandBody;

        internal void NextCommand(params object[] commandParameters);

        internal void CompleteSequence();

        void Stop();
        void Dispose();
    }
}