using System.Linq;
using System.Reflection;
using MVC.Runtime.Injectable.Attributes;

namespace MVC.Runtime.Injectable.Utils
{
    internal static class PostConstructUtils
    {
        public static void ExecutePostConstructMethod(object target)
        {
            var type = target.GetType();
            var postConstructMethods =
                type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(PostConstructAttribute), true).Length != 0)
                    .ToList();

            foreach (var postConstructMethod in postConstructMethods)
            {
                postConstructMethod.Invoke(target, null);
            }
        }
        
        public static void ExecuteDeconstructMethod(object target)
        {
            var type = target.GetType();
            var deconstructMethods =
                type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(DeconstructAttribute), true).Length != 0)
                    .ToList();

            foreach (var deconstructMethod in deconstructMethods)
            {
                deconstructMethod.Invoke(target, null);
            }
        }
    }
}