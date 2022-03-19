namespace MVC.Runtime.Function.Provider
{
    public interface IFunctionDataContainer
    {
        IFunctionDataContainer AddParams(params object[] executeParameters);
        TReturnType SetReturn<TReturnType>();
        void SetVoid();
    }
}