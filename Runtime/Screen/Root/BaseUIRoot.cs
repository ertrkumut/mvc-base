using MVC.Runtime.Root;
using MVC.Runtime.Screen.Context;

namespace MVC.Runtime.Screen.Root
{
    public class BaseUIRoot<TContext> : Root<TContext>
        where TContext : BaseUIContext, new()
    {
        
    }
}