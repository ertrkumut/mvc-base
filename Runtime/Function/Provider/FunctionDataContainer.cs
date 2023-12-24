using System;

namespace MVC.Runtime.Function.Provider
{
    internal class FunctionDataContainer : IFunctionDataContainer
    {
        public FunctionProvider FunctionProvider;
        
        internal Type FunctionType;
        internal object[] ExecuteParameters;

        public FunctionDataContainer()
        {
        }
        
        public void SetFunctionType(Type functionType)
        {
            FunctionType = functionType;
        }

        public IFunctionDataContainer AddParams(params object[] executeParameters)
        {
            ExecuteParameters = executeParameters;
            return this;
        }

        public TReturnType SetReturn<TReturnType>()
        {
            var result = FunctionProvider.ExecuteFunction<TReturnType>(this);
            return result;
        }

        public void SetVoid()
        {
            FunctionProvider.ExecuteFunction(this);
        }

        public void Dispose()
        {
            FunctionType = null;
            ExecuteParameters = null;
        }
    }
}