using System;
using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Editor.CodeGenerator.TempViews
{
    [RequireComponent(typeof(ViewInjector))]
    internal class TempView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }
        
        //@Actions
    }
}