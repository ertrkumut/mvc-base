using System.Collections.Generic;
using MVC.Runtime.Contexts;

namespace MVC.Runtime.Root
{
    public interface IRoot
    {
        void StartContext(bool forceToStart = false);
        void InitializeSubContexts();
        IContext GetContext();
        List<IContext> GetSubContexts();
        List<IContext> GetAllContexts();
        void Setup();
        void Launch(bool forceToLaunch = false);
    }
}