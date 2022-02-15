namespace MVC.Runtime.Controller.Binder
{
    public interface ICommandBinding
    {
        public object Key { get; }
        public object Value { get;}
        
        CommandExecutionType ExecutionType { get; }

        void InSequence();
        void InParallel();
    }
}