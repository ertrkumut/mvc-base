using System;
using System.Linq;
using System.Reflection;
using MVC.Runtime.Contexts;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Runtime.Injectable.Utils
{
    public static class InjectionExtensions
    {
        public static bool TryToInjectMediator(this IContext context, IMVCMediator mediator, IMVCView view)
        {
            var injectionBinder = context.InjectionBinder;

            InjectFields(mediator, view, injectionBinder);
            InjectProperties(mediator, view, injectionBinder);
            
            mediator.OnRegister();
            return true;
        }

        private static void InjectFields(object mediator, object view, InjectionBinder injectionBinder)
        {
            var viewType = view.GetType();
            var mediatorType = mediator.GetType();
            
            var injectableFields = mediatorType.GetFields(BindingFlags.Instance | 
                                                          BindingFlags.Public | 
                                                          BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(typeof(InjectAttribute)).ToList().Count != 0)
                .ToList();
            
            foreach (var injectableFieldInfo in injectableFields)
            {
                var fieldType = injectableFieldInfo.FieldType;
                var injectAttribute = injectableFieldInfo.GetCustomAttributes(typeof(InjectAttribute)).ToList()[0] as InjectAttribute;
                
                if (fieldType == viewType)
                {
                    injectableFieldInfo.SetValue(mediator, view);
                }
                else
                {
                    var injectionValue = injectionBinder.GetInstance(fieldType, injectAttribute.Name);
                    if (injectionValue == null)
                    {
                        Debug.LogError("Injection Failed! There is no injected property in container! \n ViewType: " + viewType + "\n Injection Type: " + fieldType);
                        continue;
                    }
                    
                    injectableFieldInfo.SetValue(mediator, injectionValue);
                }
            }
        }
        
        private static void InjectProperties(object mediator, object view, InjectionBinder injectionBinder)
        {
            var viewType = view.GetType();
            var mediatorType = mediator.GetType();
            
            var injectableFields = mediatorType.GetProperties(BindingFlags.Instance | 
                                                          BindingFlags.Public | 
                                                          BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(typeof(InjectAttribute)).ToList().Count != 0)
                .ToList();
            
            foreach (var injectableFieldInfo in injectableFields)
            {
                var fieldType = injectableFieldInfo.PropertyType;
                var injectAttribute = injectableFieldInfo.GetCustomAttributes(typeof(InjectAttribute)).ToList()[0] as InjectAttribute;
                
                if (fieldType == viewType)
                {
                    injectableFieldInfo.SetValue(mediator, view);
                }
                else
                {
                    var injectionValue = injectionBinder.GetInstance(fieldType, injectAttribute.Name);
                    if (injectionValue == null)
                    {
                        Debug.LogError("Injection Failed! There is no injected property in container! \n ViewType: " + viewType + "\n Injection Type: " + fieldType);
                        continue;
                    }
                    
                    injectableFieldInfo.SetValue(mediator, injectionValue);
                }
            }
        }
    }
}