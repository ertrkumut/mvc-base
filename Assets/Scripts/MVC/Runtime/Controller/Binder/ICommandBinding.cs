using System;
using System.Collections.Generic;

namespace MVC.Runtime.Controller.Binder
{
    public interface ICommandBinding
    {
        public object Key { get; }
        public object Value { get;}
        
        CommandExecutionType ExecutionType { get; }

        void InSequence();
        void InParallel();

        internal List<Type> GetBindedCommands();
    }
}