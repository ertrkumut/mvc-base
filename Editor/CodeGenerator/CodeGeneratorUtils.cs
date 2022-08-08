using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.CodeGenerator
{
    internal static class CodeGeneratorUtils
    {
        public static void CreateView(string viewName, string tempClassName, string viewPath, string tempClassPath,
            string namespaceName, List<string> actionsList)
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

            if (!Directory.Exists(viewPath)) Directory.CreateDirectory(viewPath);

            File.WriteAllLines(newViewPath, newViewContent.ToArray());
            AssetDatabase.Refresh();
        }

        public static void CreateMediator(string mediatorName, string viewName, string tempClassName,
            string mediatorPath, string tempClassPath, string namespaceName, List<string> actionsList)
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

                    newMediatorContent.RemoveAt(newMediatorContent.Count - 1);
                    continue;
                }

                newMediatorContent.Add(content);
            }

            if (!Directory.Exists(mediatorPath)) Directory.CreateDirectory(mediatorPath);

            File.WriteAllLines(newMediatorPath, newMediatorContent.ToArray());
            AssetDatabase.Refresh();
        }

        public static void CreateContext(string contextName, string tempClassName, string contextPath,
            string tempClassPath, string namespaceName)
        {
            var directoryPath = contextPath;
            var path = directoryPath + "/" + contextName + ".cs";

            var tempContextContent = File.ReadAllLines(tempClassPath);
            var newContextContent = new List<string>();

            for (var ii = 0; ii < tempContextContent.Length; ii++)
            {
                var content = tempContextContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceName;
                }
                else if (content.Contains("internal class "))
                {
                    content = content.Replace("internal class", "public class");
                    content = content.Replace(tempClassName, contextName);
                }

                newContextContent.Add(content);
            }
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            File.WriteAllLines(path, newContextContent.ToArray());
            AssetDatabase.Refresh();
        }

        public static void CreateRoot(string rootName, string contextName, string tempContextName, string tempRootName,
            string rootPath, string tempClassPath, string namespaceName)
        {
            var directoryPath = rootPath;
            var path = directoryPath + "/" + rootName + ".cs";
            
            var tempRootContent = File.ReadAllLines(tempClassPath);
            var newRootContent = new List<string>();
            
            for (var ii = 0; ii < tempRootContent.Length; ii++)
            {
                var content = tempRootContent[ii];
                if (content.Contains("namespace "))
                {
                    content = "namespace " + namespaceName;
                }
                else if (content.Contains("internal class "))
                {
                    content = content.Replace("internal class", "public class");
                    content = content.Replace(tempRootName, rootName);
                    content = content.Replace(tempContextName, contextName);
                }
            
                newRootContent.Add(content);
            }
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            
            File.WriteAllLines(path, newRootContent.ToArray());
            AssetDatabase.Refresh();
        }

        public static void BindMediationInContext(string contextPath, string viewName, string mediationName,
            string tempViewName, string tempMediationName, string viewNamespace)
        {
            var contextLines = File.ReadAllLines(contextPath);
            var newRootContent = new List<string>();
            
            newRootContent.Add("using " + viewNamespace + ";");
            for (var ii = 0; ii < contextLines.Length; ii++)
            {
                var content = contextLines[ii];
                if (content.Contains("MediationBinder.Bind<"))
                {
                    content = content.Replace(tempViewName, viewName);
                    content = content.Replace(tempMediationName, mediationName);
                }
                
                newRootContent.Add(content);
            }
            
            File.WriteAllLines(contextPath, newRootContent.ToArray());
            AssetDatabase.Refresh();
        }

        public static void ShowScreenInLaunch(string contextPath, string screenName, string tempScreenName, string screenType)
        {
            var contextLines = File.ReadAllLines(contextPath);
            var newRootContent = new List<string>();
            
            newRootContent.Add("using Runtime.Contexts.Global.Enums;");
            for (var ii = 0; ii < contextLines.Length; ii++)
            {
                var content = contextLines[ii];
                if (content.Contains("_screenModel."))
                {
                    content = content.Replace("(default", "(" + screenType);
                    content = content.Replace(tempScreenName, screenName);
                }
                
                newRootContent.Add(content);
            }
            
            File.WriteAllLines(contextPath, newRootContent.ToArray());
            AssetDatabase.Refresh();
        }
        
        public static void CreateScreenEnum(string screenType)
        {
            var gameScreenEnumPath = CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenTypeEnumPath) + "/" + CodeGeneratorStrings.ScreenTypeEnumFileName + ".cs";

            var isFileExist = File.Exists(gameScreenEnumPath);

            if (!isFileExist)
            {
                if (!Directory.Exists(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenTypeEnumPath)))
                    Directory.CreateDirectory(CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenTypeEnumPath));
                
                var lineList = new List<string>();

                var namespaceTxt = CodeGeneratorStrings.GetPath(CodeGeneratorStrings.ScreenTypeEnumPath)
                    .Replace(Application.dataPath + "/Scripts/", "")
                    .Replace("/", ".")
                    .TrimEnd('.');
                
                lineList.Add("namespace " + namespaceTxt);
                lineList.Add("{");
                    lineList.Add("\tpublic enum " + CodeGeneratorStrings.ScreenTypeEnumFileName);
                        lineList.Add("\t{");
                        lineList.Add("\t\t//[?] Dont Delete this line");
                        lineList.Add("\t}");
                lineList.Add("}");
                
                File.WriteAllLines(gameScreenEnumPath, lineList);
            }

            var fileLineArray = File.ReadAllLines(gameScreenEnumPath);
            var newLineList = new List<string>();
            for (var ii = 0; ii < fileLineArray.Length; ii++)
            {
                var line = fileLineArray[ii];

                if (line.Contains("//[?]"))
                {
                    newLineList.Add("\t\t"+ screenType + ",");
                }
                
                newLineList.Add(line);
            }
            
            File.WriteAllLines(gameScreenEnumPath, newLineList);
        }
    }
}