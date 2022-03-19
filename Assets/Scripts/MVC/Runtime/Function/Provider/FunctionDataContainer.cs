using System;

namespace MVC.Runtime.Function.Provider
{
    internal class FunctionDataContainer : IFunctionDataContainer
    {
        private FunctionProvider _functionProvider;
        
        internal Type FunctionType;
        internal object[] ExecuteParameters;

        public FunctionDataContainer(FunctionProvider functionProvider)
        {
            _functionProvider = functionProvider;
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
            var result = _functionProvider.ExecuteFunction<TReturnType>(this);
            return result;
        }

        public void SetVoid()
        {
            _functionProvider.ExecuteFunction(this);
        }

        public void Dispose()
        {
            FunctionType = null;
            ExecuteParameters = null;
        }
    }
}