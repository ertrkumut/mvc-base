using MVC.Runtime.Injectable.Components;
using MVC.Runtime.ViewMediators.View;
using UnityEngine;

namespace MVC.Examples.Views.Player
{
    [RequireComponent(typeof(ViewInjectorComponent))]
    public class PlayerControllerView : MonoBehaviour, IView
    {
        
    }
}