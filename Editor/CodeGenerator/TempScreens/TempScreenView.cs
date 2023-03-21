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
        
        /// <summary>
        /// This method runs if CustomOpeningAnimation bool is true.
        /// If you dont use custom animations delete this method.
        /// </summary>
        protected override void OpeningAnimation()
        {
            // Do some animation
            OpeningAnimationCompleted();
        }

        /// <summary>
        /// This method runs if CustomClosingAnimation bool is true.
        /// If you dont use custom animations delete this method.
        /// </summary>
        protected override void ClosingAnimation()
        {
            // Do some animation
            ClosingAnimationCompleted();
        }
    }
}