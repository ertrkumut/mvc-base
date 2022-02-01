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
            var mediatorBinder = context.MediatorBinder;
            var injectionBinder = context.InjectionBinder;

            var viewType = view.GetType();
            var mediatorType = mediator.GetType();
            
            var injectableFields = mediatorType.GetFields(BindingFlags.Instance | 
                                                          BindingFlags.Public | 
                                                          BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(typeof(InjectAttribute)) != null)
                .ToList();
            
            foreach (var injectableFieldInfo in injectableFields)
            {
                var fieldType = injectableFieldInfo.FieldType;
                if (fieldType == viewType)
                {
                    injectableFieldInfo.SetValue(mediator, view);
                }
                else
                {
                    var injectionValue = injectionBinder.GetInstance(fieldType);
                    if (injectionValue == null)
                    {
                        Debug.LogError("Injection Failed! There is no injected property in container! \n ViewType: " + viewType + "\n Injection Type: " + fieldType);
                        continue;
                    }
                    
                    injectableFieldInfo.SetValue(mediator, injectionValue);
                }
            }
            
            mediator.OnRegister();
            return true;
        }
    }
}