using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Editor.Console;
using MVC.Runtime.Attributes;
using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Controller.Sequencer;
using MVC.Runtime.Signals;
using UnityEngine;

namespace MVC.Runtime.Controller.Binder
{
    [HideInModelViewer]
    public class CommandBinder : Binder<CommandBinding>, ICommandBinder
    {
        private Dictionary<Type, List<ICommandBody>> _commandPool;
        private List<ICommandSequencer> _sequencePool;
        private List<ICommandSequencer> _activeSequenceList;

        internal IContext Context;
        
        public CommandBinder()
        {
            _commandPool = new Dictionary<Type, List<ICommandBody>>();
            
            _sequencePool = new List<ICommandSequencer>();
            _activeSequenceList = new List<ICommandSequencer>();
        }
        
        public virtual CommandBinding Bind<TSignal>(TSignal key)
            where TSignal : ISignalBody
        {
            key.InternalCallback = null;
            key.InternalCallback += SignalDispatcher;
            var binding = base.Bind(key);
            binding.SetContext(Context);
            return binding;
        }

        private void SignalDispatcher(ISignalBody signal, params object[] commandParameters)
        {
            var binding = GetBinding(signal) as ICommandBinding;
            if (binding == null)
                return;

            var sequence = GetAvailableSequence();
            sequence.Initialize(binding, this, commandParameters);
            sequence.SequenceFinished += sequencer =>
            {
                ReturnSequenceToPool(sequencer);
            };
            _activeSequenceList.Add(sequence);
            MVCConsole.Log(ConsoleLogType.Command,"Signal Dispatched: " + signal.Name);
            sequence.RunCommands();
        }

        public virtual void ReleaseCommand(ICommandBody commandBody, params object[] commandParameters)
        {
            var sequence = GetActiveSequence(commandBody);
            if (sequence == null)
            {
                Debug.LogError("RELEASE FAILED! - Command Sequence not found! \n CommandType: " + commandBody.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Command, "RELEASE FAILED! - Command Sequence not found! \n CommandType: " + commandBody.GetType().Name);
                return;
            }
            
            sequence.ReleaseCommand(commandBody, commandParameters);
        }

        public virtual void Jump<TCommandType>(ICommandBody commandBody, params object[] commandParameters)
            where TCommandType : ICommandBody 
        {
            var sequence = GetActiveSequence(commandBody);
            if (sequence == null)
            {
                Debug.LogError("JUMP FAILED! - Command Sequence not found! \n CommandType: " + commandBody.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Command, "JUMP FAILED! - Command Sequence not found! \n CommandType: " + commandBody.GetType().Name);
                return;
            }
            
            sequence.JumpCommand<TCommandType>(commandBody, commandParameters);
        }
        
        public virtual void StopCommand(ICommandBody commandBody)
        {
            var sequence = GetActiveSequence(commandBody);
            if (sequence == null)
            {
                Debug.LogError("COMMAND STOP FAILED! - Command Sequence not found! \n CommandType: " + commandBody.GetType().Name);
                MVCConsole.LogError(ConsoleLogType.Command, "COMMAND STOP FAILED! - Command Sequence not found! \n CommandType: " + commandBody.GetType().Name);
                return;
            }

            sequence.Stop();
        }
        
        #region SequencePool

        private ICommandSequencer GetAvailableSequence()
        {
            var availableSequencer = _sequencePool.Count != 0 ? _sequencePool[0] : null;

            if (availableSequencer == null)
                availableSequencer = new CommandSequencer();

            if (_sequencePool.Contains(availableSequencer))
                _sequencePool.Remove(availableSequencer);
                
            availableSequencer.SequenceFinished = null;
            
            return availableSequencer;
        }
        
        private void ReturnSequenceToPool(ICommandSequencer commandSequencer)
        {
            commandSequencer.Dispose();
            
            _activeSequenceList.Remove(commandSequencer);
            _sequencePool.Add(commandSequencer);
        }

        private ICommandSequencer GetActiveSequence(ICommandBody commandBody)
        {
            var sequence =
                _activeSequenceList.FirstOrDefault(x => x.CurrentCommand != null && x.CurrentCommand == commandBody);
            return sequence;
        }
        
        #endregion

        #region CommandPool

        internal ICommandBody GetCommand(Type commandType)
        {
            ICommandBody command = null;
            if (_commandPool.ContainsKey(commandType) && _commandPool[commandType].Count != 0)
            {
                command = _commandPool[commandType][0];
                _commandPool[commandType].Remove(command);
                return command;
            }
            
            command = (ICommandBody) Activator.CreateInstance(commandType);
            return command;
        }

        internal void ReturnCommandToPool(ICommandBody commandBody)
        {
            commandBody.Clean();
            var commandType = commandBody.GetType();
            
            if(!_commandPool.ContainsKey(commandType))
                _commandPool.Add(commandType, new List<ICommandBody>());

            _commandPool[commandType].Add(commandBody);
            
            MVCConsole.LogWarning(ConsoleLogType.Command, "Command Returned to Pool! - " + commandType.Name);
        }

        #endregion
    }
}