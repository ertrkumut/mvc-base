using System;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    [RequireComponent(typeof(ViewInjector))]
    internal class TempScreenView : ScreenView
    {
        //@Actions
        
        protected override void ScreenOpened()
        {
            base.ScreenOpened();
        }

        protected override void ScreenClosed()
        {
            base.ScreenClosed();
        }
    }
}