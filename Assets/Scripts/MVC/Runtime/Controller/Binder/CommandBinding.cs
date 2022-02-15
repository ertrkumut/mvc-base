using MVC.Runtime.Bind.Bindings;

namespace MVC.Runtime.Controller.Binder
{
    public class CommandBinding : Binding, ICommandBinding
    {
        public CommandExecutionType ExecutionType { get; protected set; }

        public new virtual void To<TValueType>()
            where TValueType : ICommandBody
        {
            base.To<TValueType>();
        }

        public void InSequence()
        {
            ExecutionType = CommandExecutionType.Sequence;
        }

        public void InParallel()
        {
            ExecutionType = CommandExecutionType.Parallel;
        }
    }
}