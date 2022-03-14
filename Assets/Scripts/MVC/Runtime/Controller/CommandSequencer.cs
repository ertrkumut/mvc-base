using System;
using System.Collections.Generic;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Utils;

namespace MVC.Runtime.Controller
{
    internal class CommandSequencer
    {
        public Action<CommandSequencer> SequenceFinished;

        public ICommandBody currentCommand;
        private ICommandBinding _commandBinding;

        private CommandBinder _commandBinder;

        private List<Type> _commands;

        private object[] _commandParamters;
        private int _sequenceId;

        public void Initialize(ICommandBinding commandBinding, CommandBinder commandBinder, params object[] commandParameters)
        {
            _commandBinder = commandBinder;
            _commandBinding = commandBinding;

            _commandParamters = commandParameters;
            
            _sequenceId = 0;
            _commands = commandBinding.GetBindedCommands();
        }

        public void RunCommands()
        {
            ExecuteCommand();
        }

        private void ExecuteCommand()
        {
            var commandType = GetCurrentCommandType();
            var command = _commandBinder.GetCommand(commandType);
            currentCommand = command;

            var context = _commandBinding.Context;
            context.InjectCommand(command, _commandParamters);
            
            ExecuteCommand(command);
            AutoReleaseCommand(command);
        }
        
        private void AutoReleaseCommand(ICommandBody command)
        {
            if(command.Retain)
                return;
            
            _commandBinder.ReturnCommandToPool(command);
            NextCommand();
        }

        public void ReleaseCommand(ICommandBody command, params object[] commandParameters)
        {
            _commandBinder.ReturnCommandToPool(command);
            NextCommand();
        }
        
        private void NextCommand()
        {
            _sequenceId++;
            if(!IsSequenceCompleted())
                ExecuteCommand();
            else
                SequenceCompleted();
        }

        private void ExecuteCommand(ICommandBody commandBody, params object[] parameters)
        {
            var commandType = commandBody.GetType();
            var executeMethodInfo = commandType.GetMethod("Execute");
            executeMethodInfo.Invoke(commandBody, parameters);
        }
        
        private void SequenceCompleted()
        {
            SequenceFinished?.Invoke(this);
        }

        private bool IsSequenceCompleted()
        {
            return _sequenceId >= _commands.Count;
        }
        
        private Type GetCurrentCommandType()
        {
            return _commands[_sequenceId];
        }

        public void Dispose()
        {
            SequenceFinished = null;
            _commands = null;
            _commandParamters = null;
            _sequenceId = default;
        }
    }
}