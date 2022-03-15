namespace MVC.Runtime.Function
{
    public class FunctionBody : IFunctionBody
    {
        public virtual void Dispose()
        {
        }
    }
    
    public interface IFunctionBody
    {
        void Dispose();
    }
}