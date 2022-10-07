using System.Linq;
using MVC.Runtime.Screen.Root;
using MVC.Runtime.Screen.View;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    internal class TempScreenTestRoot : BaseUIRoot<TempScreenTestContext>
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