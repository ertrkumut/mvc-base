using System;
using System.Collections.Generic;
using MVC.Runtime.Controller.Binder;

namespace MVC.Runtime.Controller
{
    internal class CommandSequencer
    {
        public Action<CommandSequencer> SequenceFinished;
        
        private CommandBinder _commandBinder;
        
        private ICommandBinding _commandBinding;
        private List<Type> _commands;

        private int _sequenceId;

        public void Initialize(ICommandBinding commandBinding, CommandBinder commandBinder)
        {
            _commandBinder = commandBinder;
            
            _sequenceId = 0;
            _commandBinding = commandBinding;
            _commands = _commandBinding.GetBindedCommands();
        }

        public void Clear()
        {
            _sequenceId = default;
            _commands = default;
            _commandBinding = default;
        }
        
        public void RunCommands()
        {
            var commandType = GetCurrentCommandType();
            var command = _commandBinder.GetCommand(commandType);
            ExecuteCommand(command);
            _commandBinder.ReturnCommandToPool(command);
        }

        private void ExecuteCommand(ICommandBody commandBody, params object[] parameters)
        {
            var commandType = commandBody.GetType();
            var executeMethodInfo = commandType.GetMethod("Execute");
            executeMethodInfo.Invoke(commandBody, null);
        }
        
        private void SequenceCompleted()
        {
            SequenceFinished?.Invoke(this);
        }
        
        private Type GetCurrentCommandType()
        {
            return _commands[_sequenceId];
        }
    }
}