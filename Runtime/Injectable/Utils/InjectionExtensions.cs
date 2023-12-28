using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Editor.Console;
using MVC.Runtime.Console;
using MVC.Runtime.Contexts;
using MVC.Runtime.Controller;
using MVC.Runtime.Function;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Utils
{
    public static class InjectionExtensions
    {
        private static Dictionary<Type, CashedInjectableData> _cashedInjectableData = new();

        private class CashedInjectableData
        {
            public Type Main;
            public Dictionary<Type, List<FieldInfo>> CashedFieldInfoList = new();
            public Dictionary<Type, List<PropertyInfo>> CashedPropertyInfoList = new();
        }
        
        internal static bool TryToInjectObject(this InjectionBinding binding)
        {
            var injectableFields = GetInjectableFieldInfoList<InjectAttribute>(binding.Value);
            var injectableProperties = GetInjectablePropertyInfoList<InjectAttribute>(binding.Value);
            
            injectableFields.ForEach(x => SetInjectedValue(binding.Value, binding.BindedContext, x));
            injectableProperties.ForEach(x => SetInjectedValue(binding.Value, binding.BindedContext, x));
            
            return true;
        }
        internal static bool TryToInjectObject(this IContext context, object injectedObject)
        {
            var injectableFields = GetInjectableFieldInfoList<InjectAttribute>(injectedObject);
            var injectableProperties = GetInjectablePropertyInfoList<InjectAttribute>(injectedObject);
            
            injectableFields.ForEach(x => SetInjectedValue(injectedObject, context, x));
            injectableProperties.ForEach(x => SetInjectedValue(injectedObject, context, x));
            
            return true;
        }

        #region InjectMediators

        internal static bool TryToInjectMediator(this IContext context, IMediator mediator, IView view)
        {
            InjectMediatorFields(mediator, view, context);
            InjectMediatorProperties(mediator, view, context);
            
            mediator.OnRegister();
            return true;
        }

        private static void InjectMediatorFields(object mediator, object view, IContext context)
        {
            var viewType = view.GetType();

            var injectableFields = GetInjectableFieldInfoList<InjectAttribute>(mediator);
            
            foreach (var injectableFieldInfo in injectableFields)
            {
                var fieldType = injectableFieldInfo.FieldType;

                if (fieldType == viewType || viewType.IsSubclassOf(fieldType))
                {
                    injectableFieldInfo.SetValue(mediator, view);
                }
                else if (fieldType.IsInterface && fieldType.IsAssignableFrom(viewType))
                {
                    injectableFieldInfo.SetValue(mediator, view);
                }
                else
                {
                    SetInjectedValue(mediator, context, injectableFieldInfo);
                }
            }
        }
        
        private static void InjectMediatorProperties(object mediator, object view, IContext context)
        {
            var viewType = view.GetType();

            var injectablePropertyList = GetInjectablePropertyInfoList<InjectAttribute>(mediator);
            
            foreach (var injectableProperty in injectablePropertyList)
            {
                var fieldType = injectableProperty.PropertyType;

                if (fieldType == viewType || viewType.IsSubclassOf(fieldType))
                {
                    injectableProperty.SetValue(mediator, view);
                }
                else if (fieldType.IsInterface && fieldType.IsAssignableFrom(viewType))
                {
                    injectableProperty.SetValue(mediator, view);
                }
                else
                {
                    SetInjectedValue(mediator, context, injectableProperty);
                }
            }
        }

        #endregion

        #region InjectFunctions

        internal static void TryToInjectFunction(this IContext context, IFunctionBody functionBody)
        {
            context.TryToInjectObject(functionBody);
        }

        #endregion
        
        #region InjectCommands

        internal static void InjectCommand(this IContext context, ICommandBody command, params object[] signalParams)
        {
            var injectableFields = GetInjectableFieldInfoList<InjectAttribute>(command);
            var injectableProperties = GetInjectablePropertyInfoList<InjectAttribute>(command);
                
            foreach (var injectableField in injectableFields)
                SetInjectedValue(command, context, injectableField);

            foreach (var injectableProperty in injectableProperties)
                SetInjectedValue(command, context, injectableProperty);

            InjectSignalParamsToCommand(context, command, signalParams);
        }

        private static void InjectSignalParamsToCommand(IContext context, ICommandBody command, params object[] signalParams)
        {
            if (signalParams == null || signalParams.Length == 0)
                return;
            
            var signalFields = GetInjectableFieldInfoList<SignalParamAttribute>(command);
            var signalProperties = GetInjectablePropertyInfoList<SignalParamAttribute>(command);

            foreach (var signalField in signalFields)
            {
                var paramType = signalField.FieldType;
                var param = signalParams.FirstOrDefault(x =>
                {
                    if(x is null)
                    {
                        Debug.LogError(
                            $"[InjectionError] SignalParam couldn't injected Command:{command.GetType().Name}");
                        return false;
                    }
                    
                    return x.GetType() == paramType;
                });
                if (param == null)
                {
                    param = signalParams.FirstOrDefault(x =>
                    {
                        if (x is null)
                            return false;

                        return paramType.IsAssignableFrom(x.GetType());
                    });
                }

                if (param == null)
                {
                    var errString =
                        "<b><color=#FF6666>► Signal Param is not found!</color></b>\n" +
                        "<b><color=#FF6666>► Command:</color><color=#FFEFD5> " + command.GetType().Name + "</color></b>\n" +
                        "<b><color=#FF6666>► ParamType:</color><color=#FFEFD5> " + paramType.Name + "</color></b>";
                    Debug.LogError(errString);
                    MVCConsole.LogError(ConsoleLogType.Command, errString);
                }
                signalField.SetValue(command, param);
            }
            
            foreach (var signalProperty in signalProperties)
            {
                var paramType = signalProperty.PropertyType;
                var param = signalParams.FirstOrDefault(x => x.GetType() == paramType);
                if (param == null)
                {
                    param = signalParams.FirstOrDefault(x => paramType.IsAssignableFrom(x.GetType()));
                }

                if (param == null)
                {
                    var errString =
                        "<b><color=#FF6666>► Signal Param is not found!</color></b>\n" +
                        "<b><color=#FF6666>► Command:</color><color=#FFEFD5> " + command.GetType().Name + "</color></b>\n" +
                        "<b><color=#FF6666>► ParamType:</color><color=#FFEFD5> " + paramType.Name + "</color></b>";
                    Debug.LogError(errString);
                    MVCConsole.LogError(ConsoleLogType.Command, errString);
                }
                signalProperty.SetValue(command, param);
            }
        }
        
        #endregion

        #region GetInjectable Fields-Properties

        private static List<FieldInfo> GetInjectableFieldInfoList<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            var instanceType = instance.GetType();
            var injectableTypes = instanceType.GetAllChildClasses();

            if(!injectableTypes.Contains(instanceType))
                injectableTypes.Add(instanceType);
            
            if(!_cashedInjectableData.ContainsKey(instanceType))
                _cashedInjectableData.Add(instanceType, new CashedInjectableData
                {
                    Main = instanceType
                });
            
            if(!_cashedInjectableData[instanceType].CashedFieldInfoList.ContainsKey(typeof(TAttribute)))
            {
                var injectableFields = new List<FieldInfo>();
                foreach (var injectableType in injectableTypes)
                {
                    var fields = injectableType
                        .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(x =>
                            x.GetCustomAttributes(typeof(TAttribute)).ToList().Count != 0)
                        .ToList();

                    injectableFields = injectableFields.Concat(fields).ToList();
                }
                
                _cashedInjectableData[instanceType].CashedFieldInfoList.Add(typeof(TAttribute), injectableFields);
                return injectableFields;
            }

            return _cashedInjectableData[instanceType].CashedFieldInfoList[typeof(TAttribute)];
        }

        private static List<PropertyInfo> GetInjectablePropertyInfoList<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            var instanceType = instance.GetType();
            var injectableTypes = instanceType.GetAllChildClasses();

            if(!injectableTypes.Contains(instanceType))
                injectableTypes.Add(instanceType);
            
            if(!_cashedInjectableData.ContainsKey(instanceType))
                _cashedInjectableData.Add(instanceType, new CashedInjectableData
                {
                    Main = instanceType
                });
            
            if(!_cashedInjectableData[instanceType].CashedPropertyInfoList.ContainsKey(typeof(TAttribute)))
            {
                var injectableProperties = new List<PropertyInfo>();
                foreach (var injectableType in injectableTypes)
                {
                    var properties = injectableType
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(x =>
                            x.GetCustomAttributes(typeof(TAttribute)).ToList().Count != 0)
                        .ToList();

                    injectableProperties = injectableProperties.Concat(properties).ToList();
                }
                
                _cashedInjectableData[instanceType].CashedPropertyInfoList.Add(typeof(TAttribute), injectableProperties);
                return injectableProperties;
            }

            return _cashedInjectableData[instanceType].CashedPropertyInfoList[typeof(TAttribute)];
        }

        #endregion

        private static void SetInjectedValue(object objectInstance, IContext context, MemberInfo injectedMemberInfo)
        {
            Type injectionType = null;
            if (injectedMemberInfo.MemberType == MemberTypes.Field)
                injectionType = (injectedMemberInfo as FieldInfo).FieldType;
            else if(injectedMemberInfo.MemberType == MemberTypes.Property)
                injectionType = (injectedMemberInfo as PropertyInfo).PropertyType;
            
            var injectionValue = context.GetInjectedObject(injectedMemberInfo);
            if (injectionValue == null)
            {
                var errString =
                "<b><color=#FF6666>► INJECTION FAILED!</color></b> There is no injected property in container!" +
                "\n<b><color=#FF6666>► Instance Type:</color><color=#FFEFD5> " + objectInstance.GetType().Name + "</color></b>" +
                "\n<b><color=#FF6666>► Injection Type:</color> " + injectionType.Name + " - " +
                injectedMemberInfo.Name + "</b>"+
                "\n<b><color=#FF6666>► Injected Context:</color> "+ context + "</b>";
                MVCConsole.LogError(ConsoleLogType.Injection, errString);
                return;
            }
            // else
            // {
            //     var errString =
            //         "<b><color=#66FF66>► INJECTION COMPLETED!</color></b>" +
            //         "\n<b><color=#66FF66>► Instance Type:</color><color=#FFEFD5> " + objectInstance.GetType().Name + "</color></b>" +
            //         "\n<b><color=#66FF66>► Injection Type:</color> " + injectionType.Name + " - " +
            //         injectedMemberInfo.Name + "</b>"+
            //         "\n<b><color=#66FF66>► Injected Context:</color> "+ context + "</b>";
            //     Debug.Log(errString);
            //     MVCConsole.Log(ConsoleLogType.Injection, errString);
            // }
                
            
            if (injectedMemberInfo.MemberType == MemberTypes.Field)
                (injectedMemberInfo as FieldInfo).SetValue(objectInstance, injectionValue);
            else if(injectedMemberInfo.MemberType == MemberTypes.Property)
                (injectedMemberInfo as PropertyInfo).SetValue(objectInstance, injectionValue);
        }
        
        private static object GetInjectedObject(this IContext context, MemberInfo memberInfo)
        {
            Type injectionType = null;
            if (memberInfo.MemberType == MemberTypes.Field)
                injectionType = (memberInfo as FieldInfo).FieldType;
            else if(memberInfo.MemberType == MemberTypes.Property)
                injectionType = (memberInfo as PropertyInfo).PropertyType;
            
            var injectAttribute = memberInfo.GetCustomAttributes(typeof(InjectAttribute)).ToList()[0] as InjectAttribute;
            var crossContextInjectionBinder = context.InjectionBinderCrossContext;

            var allContexts = context.AllContexts;
            
            object injectionValue = null;
            foreach (var cntxt in allContexts)
            {
                injectionValue = cntxt.InjectionBinder.GetInstance(injectionType, injectAttribute.Name);
                if (injectionValue != null)
                    return injectionValue;
            }
            
            injectionValue = crossContextInjectionBinder.GetInstance(injectionType, injectAttribute.Name);

            return injectionValue;
        }

        internal static List<Type> GetAllChildClasses(this Type type)
        {
            var cashData = InjectionCashing.GetCashData(type);

            if (cashData != null)
                return cashData.Children;
            
            var childTypes = Assembly
                .GetAssembly(type)
                .GetTypes()
                .Where(x => x.IsAssignableFrom(type) && !x.IsInterface)
                .ToList();

            InjectionCashing.AddCashData(type, childTypes);
            
            return childTypes;
        }
    }
}