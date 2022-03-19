using System;

namespace MVC.Runtime.Function.Provider
{
    internal class FunctionDataContainer : IFunctionDataContainer
    {
        public Action OnFunctionExecuted;

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
            OnFunctionExecuted?.Invoke();
            return result;
        }

        public void SetVoid()
        {
            _functionProvider.ExecuteFunction(this);
            OnFunctionExecuted?.Invoke();
        }

        public void Dispose()
        {
            OnFunctionExecuted = null;
            FunctionType = null;
            ExecuteParameters = null;
        }
    }
}