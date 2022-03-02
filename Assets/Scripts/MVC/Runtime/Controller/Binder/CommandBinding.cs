using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Runtime.Bind.Bindings;

namespace MVC.Runtime.Controller.Binder
{
    public class CommandBinding : Binding, ICommandBinding
    {
        public CommandExecutionType ExecutionType { get; protected set; }

        public new virtual CommandBinding To<TValueType>()
            where TValueType : ICommandBody
        {
            base.To<TValueType>();
            return this;
        }

        public void InSequence()
        {
            ExecutionType = CommandExecutionType.Sequence;
        }

        public void InParallel()
        {
            ExecutionType = CommandExecutionType.Parallel;
        }

        List<Type> ICommandBinding.GetBindedCommands()
        {
            return (Value as List<object>)
                .Cast<Type>()
                .ToList();
        }

        public override void Clear()
        {
            ExecutionType = CommandExecutionType.Parallel;
            base.Clear();
        }
    }
}