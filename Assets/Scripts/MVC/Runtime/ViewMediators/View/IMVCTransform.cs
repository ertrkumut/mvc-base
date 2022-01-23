using UnityEngine;

namespace MVC.Runtime.ViewMediators.View
{
    public interface IMVCTransform
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}