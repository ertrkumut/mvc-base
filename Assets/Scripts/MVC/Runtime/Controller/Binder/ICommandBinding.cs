using System;
using System.Collections.Generic;
using MVC.Runtime.Contexts;

namespace MVC.Runtime.Controller.Binder
{
    public interface ICommandBinding
    {
        public object Key { get; }
        public object Value { get;}
        
        IContext Context { get; }
        
        CommandExecutionType ExecutionType { get; }

        void InSequence();
        void InParallel();

        internal List<Type> GetBindedCommands();
    }
}