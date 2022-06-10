using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

                if (fieldType == viewType)
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

                if (fieldType == viewType)
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
            {
                SetInjectedValue(command, context, injectableField);
            }

            foreach (var injectableProperty in injectableProperties)
            {
                SetInjectedValue(command, context, injectableProperty);
            }

            InjectSignalParamsToCommand(context, command, signalParams);
        }

        private static void InjectSignalParamsToCommand(IContext context, ICommandBody command, params object[] signalParams)
        {
            var signalFields = GetInjectableFieldInfoList<SignalParamAttribute>(command);
            var signalProperties = GetInjectablePropertyInfoList<SignalParamAttribute>(command);

            foreach (var signalField in signalFields)
            {
                var paramType = signalField.FieldType;
                var param = signalParams.FirstOrDefault(x => x.GetType() == paramType);
                signalField.SetValue(command, param);
            }
            
            foreach (var signalProperty in signalProperties)
            {
                var paramType = signalProperty.PropertyType;
                var param = signalParams.FirstOrDefault(x => x.GetType() == paramType);
                signalProperty.SetValue(command, param);
            }
        }
        
        #endregion

        #region GetInjectable Fields-Properties

        private static List<FieldInfo> GetInjectableFieldInfoList<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            var injectableTypes = instance.GetType().GetAllChildClasses();
            
            if(!injectableTypes.Contains(instance.GetType()))
                injectableTypes.Add(instance.GetType());
            
            var injectableFields = new List<FieldInfo>();
            foreach (var injectableType in injectableTypes)
            {
                var fields = injectableType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => 
                        x.GetCustomAttributes(typeof(TAttribute)).ToList().Count != 0)
                    .ToList();

                injectableFields = injectableFields.Concat(fields).ToList();
            }

            return injectableFields;
        }

        private static List<PropertyInfo> GetInjectablePropertyInfoList<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            var injectableTypes = instance.GetType().GetAllChildClasses();

            if(!injectableTypes.Contains(instance.GetType()))
                injectableTypes.Add(instance.GetType());
            
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

            return injectableProperties;
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
                Debug.LogError("Injection Failed! There is no injected property in container! " +
                               "\n Instance Type: " + objectInstance.GetType().Name + 
                               "\n Injection Type: " + injectionType.Name + " - " + injectedMemberInfo.Name);
            }
            
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
            
            var injectionBinder = context.InjectionBinder;
            var crossContextInjectionBinder = context.InjectionBinderCrossContext;
            
            var injectionValue = injectionBinder.GetInstance(injectionType, injectAttribute.Name);
            if (injectionValue == null)
                injectionValue = crossContextInjectionBinder.GetInstance(injectionType, injectAttribute.Name);

            return injectionValue;
        }

        private static List<Type> GetAllChildClasses(this Type type)
        {
            var childTypes = Assembly
                .GetAssembly(type)
                .GetTypes()
                .Where(x => x.IsAssignableFrom(type) && !x.IsInterface)
                .ToList();

            return childTypes;
        }
    }
}