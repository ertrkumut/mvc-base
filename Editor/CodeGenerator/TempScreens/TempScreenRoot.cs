#if UNITY_EDITOR
using System.Linq;
using MVC.Runtime.Screen.Root;
using MVC.Runtime.Screen.View;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    internal class TempScreenRoot : BaseUIRoot<TempScreenContext>
    {
        protected override void AfterCreateBeforeStartContext()
        {
            base.AfterCreateBeforeStartContext();

            var screens = FindObjectsOfType<ScreenBody>()
                .ToList();

            for (var ii = 0; ii < screens.Count; ii++)
            {
                var screen = screens[ii];
                Destroy(screen.gameObject);
            }
        }
    }
}
#endif