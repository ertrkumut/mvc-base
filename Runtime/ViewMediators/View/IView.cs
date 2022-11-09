namespace MVC.Runtime.ViewMediators.View
{
    public interface IView : ITransform
    {
        bool IsRegistered { get; set; }
    }
}