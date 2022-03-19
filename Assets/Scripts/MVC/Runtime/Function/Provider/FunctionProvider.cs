using System;
using System.Collections.Generic;
using MVC.Runtime.Attributes;

namespace MVC.Runtime.Function.Provider
{
    [HideFromModelViewer]
    public class FunctionProvider : IFunctionProvider
    {
        private List<FunctionDataContainer> _functionDataContainerPool;
        private Dictionary<Type, List<IFunctionBody>> _functionPool;

        public FunctionProvider()
        {
            _functionDataContainerPool = new List<FunctionDataContainer>();
            _functionPool = new Dictionary<Type, List<IFunctionBody>>();
        }

        private FunctionDataContainer GetFunctionDataContainer()
        {
            var availableFunctionDataContainer = _functionDataContainerPool.Count != 0 
                ? _functionDataContainerPool[0] : null;

            if (availableFunctionDataContainer == null)
                availableFunctionDataContainer = new FunctionDataContainer(this);
            else
                _functionDataContainerPool.Remove(availableFunctionDataContainer);

            return availableFunctionDataContainer;
        }
        
        public IFunctionDataContainer Execute<TFunctionType>() where TFunctionType : IFunctionBody
        {
            var functionType = typeof(TFunctionType);
            var functionDataContainer = GetFunctionDataContainer();
            functionDataContainer.SetFunctionType(functionType);

            functionDataContainer.OnFunctionExecuted += () =>
            {
                functionDataContainer.Dispose();
                _functionDataContainerPool.Add(functionDataContainer);
            };
            
            return functionDataContainer;
        }

        internal void ExecuteFunction(FunctionDataContainer functionDataContainer)
        {
            var function = GetFunction(functionDataContainer);
            var executeMethodInfo = functionDataContainer.FunctionType.GetMethod("Execute");
            executeMethodInfo.Invoke(function, functionDataContainer.ExecuteParameters);
        }

        internal TReturnType ExecuteFunction<TReturnType>(FunctionDataContainer functionDataContainer)
        {
            var function = GetFunction(functionDataContainer);
            var executeMethodInfo = functionDataContainer.FunctionType.GetMethod("Execute");
            var result = executeMethodInfo.Invoke(function, functionDataContainer.ExecuteParameters);
            return (TReturnType) result;
        }

        private IFunctionBody GetFunction(FunctionDataContainer functionDataContainer)
        {
            var functionType = functionDataContainer.FunctionType;
            
            if(!_functionPool.ContainsKey(functionType))
                _functionPool.Add(functionType, new List<IFunctionBody>());

            var poolList = _functionPool[functionType];
            IFunctionBody function;
            
            if (poolList.Count != 0)
            {
                function = poolList[0];
                _functionPool[functionType].Remove(function);
                return function;
            }

            function = (IFunctionBody) Activator.CreateInstance(functionType);
            
            return function;
        }
    }
}