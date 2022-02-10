using UnityEngine;

namespace MVC.Runtime.ViewMediators.View
{
    public interface ITransform
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}