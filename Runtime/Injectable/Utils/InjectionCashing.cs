using System;
using System.Collections.Generic;

namespace MVC.Runtime.Injectable.Utils
{
    public static class InjectionCashing
    {
        private static Dictionary<Type, InjectionCashData> CashDataList = new();

        public static void AddCashData(Type type, List<Type> children)
        {
            if(CashDataList.ContainsKey(type))
                return;
            
            CashDataList.Add(type, new InjectionCashData
            {
                Main = type,
                Children = children
            });
        }

        public static InjectionCashData GetCashData(Type type)
        {
            return CashDataList.ContainsKey(type) ? CashDataList[type] : null;
        }
    }

    public class InjectionCashData
    {
        public Type Main;
        public List<Type> Children;
    }
}