using System;
using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.Screen;
using MVC.Runtime.Screen.View;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.TempScreens
{
    [RequireComponent(typeof(ViewInjector))]
    internal class TempScreenView : ScreenView
    {
        //@Actions
        
        [Inject] protected IScreenModel _screenModel { get; set; }
        
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