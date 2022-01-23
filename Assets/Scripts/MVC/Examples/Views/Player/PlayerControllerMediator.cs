using MVC.Runtime.Injectable.Attributes;
using MVC.Runtime.ViewMediators.Mediator;
using UnityEngine;

namespace MVC.Examples.Views.Player
{
    public class PlayerControllerMediator : MonoBehaviour, IMVCMediator
    {
        [Inject] private PlayerControllerView _view { get; set; }
    }
}