using System;
using System.Collections;
using System.Collections.Generic;
using MVC.Editor.Console;
using MVC.Function.AsyncFunctions.DataContainer;
using MVC.Runtime.Attributes;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Function.AsyncFunctions;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Injectable.Utils;
using MVC.Runtime.Provider.Coroutine;
using UnityEngine;

namespace MVC.Runtime.Function.Provider
{
    [HideInModelViewer]
    public class FunctionProvider : IFunctionProvider
    {
        [Inject] private ICoroutineProvider _coroutineProvider { get; set; }

        private Dictionary<Type, List<FunctionDataContainer>> _functionDataContainerPool; 
        private Dictionary<Type, List<IFunctionBody>> _functionPool;

        internal IContext Context;
        
        public FunctionProvider()
        {
            _functionDataContainerPool = new Dictionary<Type, List<FunctionDataContainer>>();
            _functionPool = new Dictionary<Type, List<IFunctionBody>>();
        }

        private FunctionDataContainer GetFunctionDataContainer<TDataContainerType>() where TDataContainerType : FunctionDataContainer
        {
            var dataContainerType = typeof(TDataContainerType);
            if(!_functionDataContainerPool.ContainsKey(dataContainerType))
                _functionDataContainerPool.Add(dataContainerType, new List<FunctionDataContainer>());
            
            var availableFunctionDataContainer = _functionDataContainerPool[dataContainerType].Count != 0 
                ? _functionDataContainerPool[dataContainerType][0] : null;

            if (availableFunctionDataContainer == null)
            {
                availableFunctionDataContainer = Activator.CreateInstance<TDataContainerType>();
                availableFunctionDataContainer.FunctionProvider = this;

                if (availableFunctionDataContainer is AsyncFunctionDataContainerBase asyncDataContainer)
                    asyncDataContainer.CoroutineProvider = _coroutineProvider;
            }
            else
                _functionDataContainerPool[dataContainerType].Remove(availableFunctionDataContainer);

            return availableFunctionDataContainer;
        }
        
        public IFunctionDataContainer Execute<TFunctionType>() where TFunctionType : IFunctionBody
        {
            var functionType = typeof(TFunctionType);
            var functionDataContainer = GetFunctionDataContainer<FunctionDataContainer>();
            functionDataContainer.SetFunctionType(functionType);

            return functionDataContainer;
        }
        
        public IAsyncFunctionDataContainer ExecuteAsync<TFunctionType>() where TFunctionType : IAsyncFunction
        {
            var functionType = typeof(TFunctionType);
            var functionDataContainer = GetFunctionDataContainer<AsyncFunctionDataContainer>();
            functionDataContainer.SetFunctionType(functionType);

            return functionDataContainer as IAsyncFunctionDataContainer;
        }

        public IAsyncFunctionDataContainer<TParam1> ExecuteAsync<TFunctionType, TParam1>() where TFunctionType : IAsyncFunction<TParam1>
        {
            var functionType = typeof(TFunctionType);
            var functionDataContainer = GetFunctionDataContainer<AsyncFunctionDataContainer<TParam1>>();
            functionDataContainer.SetFunctionType(functionType);

            return functionDataContainer as IAsyncFunctionDataContainer<TParam1>;
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
            _functionDataContainerPool[functionDataContainer.GetType()].Add(functionDataContainer);
        }

        internal TReturnType ExecuteFunction<TReturnType>(FunctionDataContainer functionDataContainer)
        {
            var function = GetFunction(functionDataContainer);
            var executeMethodInfo = functionDataContainer.FunctionType.GetMethod("Execute");
            
            Context.TryToInjectFunction(function);
            var result = executeMethodInfo.Invoke(function, functionDataContainer.ExecuteParameters);
            MVCConsole.LogWarning(ConsoleLogType.Function, "Function Executed! " + function.GetType().Name);
            
            functionDataContainer.Dispose();
            _functionDataContainerPool[functionDataContainer.GetType()].Add(functionDataContainer);
            ReturnFunctionToPool(function);
            return (TReturnType) result;
        }

        internal IEnumerator ExecuteAsyncFunction(FunctionDataContainer functionDataContainer)
        {
            var function = GetFunction(functionDataContainer) as AsyncFunctionBody;
            var functionCompletedCallback = functionDataContainer.GetType().GetProperty("FunctionCompletedCallback").GetValue(functionDataContainer);
            
            function.GetType().GetProperty("FunctionCompletedCallback").SetValue(function, functionCompletedCallback);
            Context.TryToInjectFunction(function);
            yield return function.Execute();
            MVCConsole.LogWarning(ConsoleLogType.Function, "Function Executed! " + function.GetType().Name);
            ReturnFunctionToPool(function);
            functionDataContainer.Dispose();
            _functionDataContainerPool[functionDataContainer.GetType()].Add(functionDataContainer);
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