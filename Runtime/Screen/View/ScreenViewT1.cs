namespace MVC.Screen.View
{
    public class ScreenView<TParam1> : ScreenBody, IScreenView<TParam1>
    {
        public TParam1 Param1 { get; set; }

        internal override void InitializeScreenParams(params object[] screenParams)
        {
            Param1 = (TParam1) screenParams[0];
        }
    }

    public interface IScreenView<TParam1> : IScreenBody
    {
        TParam1 Param1 { get; set; }
    }
}