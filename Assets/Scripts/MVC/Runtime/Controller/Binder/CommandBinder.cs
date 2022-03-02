using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Bind.Binders;
using MVC.Runtime.Signals;
using UnityEngine;

namespace MVC.Runtime.Controller.Binder
{
    public class CommandBinder : Binder<CommandBinding>
    {
        private Dictionary<Type, List<ICommandBody>> _commandPool;
        private List<CommandSequencer> _sequencePool;
        private List<CommandSequencer> _activeSequenceList;

        public CommandBinder()
        {
            _commandPool = new Dictionary<Type, List<ICommandBody>>();
            
            _sequencePool = new List<CommandSequencer>();
            _activeSequenceList = new List<CommandSequencer>();
        }
        
        public new virtual CommandBinding Bind<TSignal>(TSignal key)
            where TSignal : ISignalBody
        {
            key.InternalCallback = null;
            key.InternalCallback += SignalDispatcher;
            var binding = base.Bind(key);
            return binding;
        }

        private void SignalDispatcher(ISignalBody signal, params object[] commandParameters)
        {
            var binding = GetBinding(signal) as ICommandBinding;
            if (binding == null)
                return;

            var sequence = GetAvailableSequence();
            sequence.Initialize(binding, this);
            sequence.SequenceFinished += sequencer =>
            {
                ReturnSequenceToPool(sequencer);
            };
            _activeSequenceList.Add(sequence);
            sequence.RunCommands();
        }

        public void ReleaseCommand(ICommandBody commandBody, params object[] commandParameters)
        {
            var sequence = GetActiveSequence(commandBody);
            if (sequence == null)
                return;
            
            sequence.ReleaseCommand(commandBody, commandParameters);
        }
        
        #region SequencePool

        private CommandSequencer GetAvailableSequence()
        {
            var availableSequencer = _sequencePool.Count != 0 ? _sequencePool[0] : null;

            if (availableSequencer == null)
                availableSequencer = new CommandSequencer();

            availableSequencer.SequenceFinished = null;
            
            return availableSequencer;
        }
        
        private void ReturnSequenceToPool(CommandSequencer commandSequencer)
        {
            commandSequencer.SequenceFinished = null;
            
            _activeSequenceList.Remove(commandSequencer);
            _sequencePool.Add(commandSequencer);
        }

        private CommandSequencer GetActiveSequence(ICommandBody commandBody)
        {
            var sequence =
                _activeSequenceList.FirstOrDefault(x => x.currentCommand != null && x.currentCommand == commandBody);
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
        }

        #endregion
    }
}