using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Runtime.Contexts;

namespace MVC.Runtime.Root.Utils
{
    public static class AssemblyExtensions
    {
        public static Assembly[] GetAssemblies()
        {
            var currentAssembly = typeof(Context).Assembly;
            
            var assemblyList = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x != currentAssembly)
                .ToArray();

            return assemblyList;
        }

        public static List<Type> GetTypesInAllAssemblies()
        {
            var assemblies = GetAssemblies();
            var typeList = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .ToList();

            return typeList;
        }

        public static List<Type> GetAllContextTypes()
        {
            var contextList = GetTypesInAllAssemblies()
                .Where(type => type.IsSubclassOf(typeof(Context)))
                .ToList();

            return contextList;
        }

        public static List<Type> GetAllRootTypes()
        {
            var rootList = GetTypesInAllAssemblies()
                .Where(type => type.IsSubclassOf(typeof(RootBase)))
                .ToList();

            return rootList;
        }
    }
}