using System;
using System.Collections.Generic;
using MVC.Editor.Console;
using MVC.Runtime.Attributes;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Injectable.Utils;
using UnityEngine;

namespace MVC.Runtime.Function.Provider
{
    [HideInModelViewer]
    public class FunctionProvider : IFunctionProvider
    {
        private List<FunctionDataContainer> _functionDataContainerPool;
        private Dictionary<Type, List<IFunctionBody>> _functionPool;

        internal IContext Context;
        
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

            return functionDataContainer;
        }

        internal void ExecuteFunction(FunctionDataContainer functionDataContainer)
        {
            var function = GetFunction(functionDataContainer);
            var executeMethodInfo = functionDataContainer.FunctionType.GetMethod("Execute");
            
            Context.TryToInjectFunction(function);
            executeMethodInfo.Invoke(function, functionDataContainer.ExecuteParameters);
            MVCConsole.LogWarning(ConsoleLogType.Function, "Function Executed! " + function.GetType().Name);
            ReturnFunctionToPool(function);
            functionDataContainer.Dispose();
            _functionDataContainerPool.Add(functionDataContainer);
        }

        internal TReturnType ExecuteFunction<TReturnType>(FunctionDataContainer functionDataContainer)
        {
            var function = GetFunction(functionDataContainer);
            var executeMethodInfo = functionDataContainer.FunctionType.GetMethod("Execute");
            
            Context.TryToInjectFunction(function);
            var result = executeMethodInfo.Invoke(function, functionDataContainer.ExecuteParameters);
            MVCConsole.LogWarning(ConsoleLogType.Function, "Function Executed! " + function.GetType().Name);
            
            functionDataContainer.Dispose();
            _functionDataContainerPool.Add(functionDataContainer);
            ReturnFunctionToPool(function);
            return (TReturnType) result;
        }

        private void ReturnFunctionToPool(IFunctionBody functionBody)
        {
            var functionType = functionBody.GetType();
            if(!_functionPool.ContainsKey(functionType))
                _functionPool.Add(functionType, new List<IFunctionBody>());
            
            functionBody.Dispose();
            _functionPool[functionType].Add(functionBody);
            
            MVCConsole.LogWarning(ConsoleLogType.Function, "Function Returned to Pool! " + functionType.Name);
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
            
            MVCConsole.LogWarning(ConsoleLogType.Function, "Function Created! " + functionType.Name);
            function = (IFunctionBody) Activator.CreateInstance(functionType);
            
            return function;
        }
    }
}