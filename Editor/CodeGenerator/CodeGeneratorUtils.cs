using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace MVC.Editor.CodeGenerator
{
    internal static class CodeGeneratorUtils
    {
        public static void CreateView(string viewName, string tempClassName, string viewPath, string tempClassPath, string namespaceName, List<string> actionsList)
        {
            var newViewPath = viewPath + "/" + viewName + ".cs";

            var tempViewContent = File.ReadAllLines(tempClassPath);
            var newViewContent = new List<string>();
            
            for (var ii = 0; ii < tempViewContent.Length; ii++)
            {
                var content = tempViewContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceName;
                }
                else if (content.Contains("internal class "))
                {
                    content = content.Replace("internal class", "public class");
                    content = content.Replace(tempClassName, viewName);
                }
                else if (content.Contains("//@Actions"))
                {
                    foreach (var actionName in actionsList)
                    {
                        newViewContent.Add("\t\tpublic Action " + actionName + ";");
                    }
                    continue;
                }
                
                newViewContent.Add(content);
            }

            if (!Directory.Exists(viewPath))
                Directory.CreateDirectory(viewPath);
            
            File.WriteAllLines(newViewPath, newViewContent.ToArray());
            AssetDatabase.Refresh();
        }

        public static void CreateMediator(string mediatorName, string viewName, string tempClassName, string mediatorPath, string tempClassPath, string namespaceName, List<string> actionsList)
        {
            var newMediatorPath = mediatorPath + "/" + mediatorName + ".cs";

            var tempMediatorContent = File.ReadAllLines(tempClassPath);
            var newMediatorContent = new List<string>();
            
            for (var ii = 0; ii < tempMediatorContent.Length; ii++)
            {
                var content = tempMediatorContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceName;
                }
                else if (content.Contains("internal class "))
                {
                    content = content.Replace("internal class", "public class");
                    content = content.Replace(tempClassName, mediatorName);
                }
                else if (content.Contains("[Inject]"))
                {
                    content = "\t\t[Inject] private " + viewName + " _view { get; set; }";
                }
                else if (content.Contains("//@Register"))
                {
                    foreach (var actionName in actionsList)
                    {
                        var line = "\t\t\t_view." + actionName + " += " + actionName + "Listener;";
                        newMediatorContent.Add(line);
                    }
                    continue;
                }
                else if (content.Contains("//@Remove"))
                {
                    foreach (var actionName in actionsList)
                    {
                        var line = "\t\t\t_view." + actionName + " -= " + actionName + "Listener;";
                        newMediatorContent.Add(line);
                    }
                    continue;
                }
                else if (content.Contains("//@Methods"))
                {
                    foreach (var actionName in actionsList)
                    {
                        var line = "\t\tprivate void " + actionName + "Listener()";
                        newMediatorContent.Add(line);
                        newMediatorContent.Add("\t\t{");
                        newMediatorContent.Add("\t\t}");
                        newMediatorContent.Add("");
                    }
                    newMediatorContent.RemoveAt(newMediatorContent.Count-1);
                    continue;
                }
                
                newMediatorContent.Add(content);
            }
            
            if (!Directory.Exists(mediatorPath))
                Directory.CreateDirectory(mediatorPath);
            
            File.WriteAllLines(newMediatorPath, newMediatorContent.ToArray());
            AssetDatabase.Refresh();
        }
    }
}