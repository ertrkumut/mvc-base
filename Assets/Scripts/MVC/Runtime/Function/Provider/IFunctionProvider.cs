namespace MVC.Runtime.Function.Provider
{
    public interface IFunctionProvider
    {
        IFunctionDataContainer Execute<TFunctionType>() where TFunctionType : IFunctionBody;
    }
}