using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Controller.Binder;
using MVC.Runtime.Injectable.Utils;
using UnityEngine;

namespace MVC.Runtime.Controller
{
    internal class CommandSequencer
    {
        public Action<CommandSequencer> SequenceFinished;

        private bool _sequenceCompleted;
        public ICommandBody currentCommand;
        private ICommandBinding _commandBinding;

        private CommandBinder _commandBinder;

        private List<Type> _commands;

        private object[] _signalParameters;
        private int _sequenceId;

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
            ExecuteCommand();
        }

        internal virtual void ExecuteCommand(params object[] commandParameters)
        {
            if(_sequenceCompleted)
                return;
            
            var commandType = GetCurrentCommandType();
            var command = _commandBinder.GetCommand(commandType);
            currentCommand = command;

            var context = _commandBinding.Context;
            context.InjectCommand(command, _signalParameters);

            ExecuteCommand(command, commandParameters);
            AutoReleaseCommand(command);
        }
        
        private void ExecuteCommand(ICommandBody commandBody, params object[] parameters)
        {
            var commandType = commandBody.GetType();
            var executeMethodInfo = commandType.GetMethod("Execute");
            executeMethodInfo.Invoke(commandBody, parameters);
        }
        
        internal virtual void AutoReleaseCommand(ICommandBody command)
        {
            if(command.IsRetain)
                return;
            
            _commandBinder.ReturnCommandToPool(command);
            
            var next = !(_sequenceCompleted || _commands.Last() == command.GetType());
            if(next)
                NextCommand();
        }

        public virtual void ReleaseCommand(ICommandBody command, params object[] commandParameters)
        {
            _commandBinder.ReturnCommandToPool(command);
            NextCommand(commandParameters);
        }

        public virtual void JumpCommand<TCommandType>(ICommandBody command, params object[] commandParameters)
            where TCommandType : ICommandBody
        {
            _commandBinder.ReturnCommandToPool(command);
            var nextCommandType = FindCommand<TCommandType>();
            if (nextCommandType == null)
            {
                Debug.LogError("JUMP FAILED! - Command Type not found! \n Command Type: " + command.GetType().Name);
                return;
            }

            _sequenceId = FindCommandIndex(nextCommandType);
            ExecuteCommand(commandParameters);
        }
        
        internal virtual void NextCommand(params object[] commandParameters)
        {
            _sequenceId++;
            if(!IsSequenceCompleted())
                ExecuteCommand(commandParameters);
            else
                SequenceCompleted();
        }

        internal virtual void SequenceCompleted()
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
            SequenceCompleted();
        }
        
        public virtual void Dispose()
        {
            SequenceFinished = null;
            _commands = null;
            _signalParameters = null;
            _sequenceId = default;
        }
    }
}