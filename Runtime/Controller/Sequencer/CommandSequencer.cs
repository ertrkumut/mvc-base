using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Utils;
using UnityEngine;

namespace MVC.Runtime.Controller.Sequencer
{
    internal class CommandSequencer : ICommandSequencer
    {
        public Action<ICommandSequencer> SequenceFinished { get; set; }

        protected bool _sequenceCompleted;
        public ICommandBody CurrentCommand { get; set; }
        protected ICommandBinding _commandBinding;

        protected CommandBinder _commandBinder;

        protected List<Type> _commands;

        protected object[] _signalParameters;
        protected int _sequenceId;

        public virtual void Initialize(ICommandBinding commandBinding, CommandBinder commandBinder, params object[] signalParameters)
        {
            _sequenceCompleted = false;
            
            _commandBinder = commandBinder;
            _commandBinding = commandBinding;

            _signalParameters = signalParameters;
            
            _sequenceId = 0;
            _commands = commandBinding.GetBindedCommands();
        }

        public virtual void RunCommands()
        {
            (this as ICommandSequencer).ExecuteCommand();
        }

        void ICommandSequencer.ExecuteCommand(params object[] commandParameters)
        {
            if(_sequenceCompleted)
                return;
            
            var commandType = GetCurrentCommandType();
            var command = _commandBinder.GetCommand(commandType);
            CurrentCommand = command;
            
            var context = _commandBinding.Context;
            context.InjectCommand(command, _signalParameters);
            
            ExecuteCommand(command, commandParameters);
        }
        
        private void ExecuteCommand(ICommandBody command, params object[] parameters)
        {
            var commandType = command.GetType();
            
            MVCConsole.Log(ConsoleLogType.Command, "Command Executed! - " + commandType.Name);
            
            var executeMethodInfo = commandType.GetMethod("Execute");
            executeMethodInfo?.Invoke(command, parameters);

            if (_commandBinding.ExecutionType == CommandExecutionType.Parallel)
                (this as ICommandSequencer).ParallelAutoReleaseCommand(command);
            else
                (this as ICommandSequencer).SequenceAutoReleaseCommand(command);
        }

        void ICommandSequencer.ParallelAutoReleaseCommand(ICommandBody command)
        {
            if (!command.HasRetain)
                _commandBinder.ReturnCommandToPool(command);
            
            if (_commands.Last() != command.GetType())
                (this as ICommandSequencer).NextCommand();
        }
        void ICommandSequencer.SequenceAutoReleaseCommand(ICommandBody command)
        {
            if (command.HasRetain)
                return;
            
            _commandBinder.ReturnCommandToPool(command);

            var next = !(_sequenceCompleted || _commands.Last() == command.GetType());
            if(next)
                (this as ICommandSequencer).NextCommand();
        }

        public virtual void ReleaseCommand(ICommandBody command, params object[] commandParameters)
        {
            if (!command.IsRetain)
            {
                Debug.LogError("Command must be retain, if you want to call manual RELEASE! \n Command: " + command.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Command, "Command must be retain, if you want to call manual RELEASE! \n Command: " + command.GetType().Name);
                return;
            }
            
            _commandBinder.ReturnCommandToPool(command);
            
            if (_commandBinding.ExecutionType == CommandExecutionType.Sequence)
                (this as ICommandSequencer).NextCommand(commandParameters);
        }

        public virtual void JumpCommand<TCommandType>(ICommandBody command, params object[] commandParameters)
            where TCommandType : ICommandBody
        {
            if (_commandBinding.ExecutionType == CommandExecutionType.Parallel)
            {
                Debug.LogWarning("Command Execute Mode must be InSequence, if you want to call JUMP! \n Command: " + command.GetType().Name);
                MVCConsole.LogWarning(ConsoleLogType.Command, "Command Execute Mode must be InSequence, if you want to call JUMP! \n Command: " + command.GetType().Name);
                return;
            }
            
            _commandBinder.ReturnCommandToPool(command);
            var nextCommandType = FindCommand<TCommandType>();
            if (nextCommandType == null)
            {
                Debug.LogError("JUMP FAILED! - Command Type not found! \n Command Type: " + command.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Command, "JUMP FAILED! - Command Type not found! \n Command Type: " + command.GetType().Name);
                return;
            }

            MVCConsole.Log(ConsoleLogType.Command, "Command Jumped! - " + nextCommandType.Name);
            _sequenceId = FindCommandIndex(nextCommandType);
            (this as ICommandSequencer).ExecuteCommand(commandParameters);
        }
        
        void ICommandSequencer.NextCommand(params object[] commandParameters)
        {
            _sequenceId++;
            if(IsSequenceCompleted())
                (this as ICommandSequencer).CompleteSequence();
            else
                (this as ICommandSequencer).ExecuteCommand(commandParameters);
        }

        void ICommandSequencer.CompleteSequence()
        {
            _sequenceCompleted = true;
            SequenceFinished?.Invoke(this);
        }

        protected bool IsSequenceCompleted()
        {
            return _sequenceId >= _commands.Count;
        }
        
        protected Type GetCurrentCommandType()
        {
            return _commands[_sequenceId];
        }

        protected Type FindCommand<TCommandBody>()
            where TCommandBody : ICommandBody
        {
            var command = _commands.FirstOrDefault(x => x == typeof(TCommandBody));
            return command;
        }

        protected int FindCommandIndex(Type commandType)
        {
            return _commands.IndexOf(commandType);
        }

        public virtual void Stop()
        {
            (this as ICommandSequencer).CompleteSequence();
        }
        
        public virtual void Dispose()
        {
            SequenceFinished = null;
            _commands = null;
            _signalParameters = null;
            _sequenceId = default;
            
            CurrentCommand = null;
        }
    }
}