namespace MVC.Screen.View
{
    public class ScreenView<TParam1, TParam2> : ScreenBody, IScreenView<TParam1, TParam2>
    {
        public TParam1 Param1 { get; set; }
        public TParam2 Param2 { get; set; }

        internal override void InitializeScreenParams(params object[] screenParams)
        {
            Param1 = (TParam1) screenParams[0];
            Param2 = (TParam2) screenParams[1];
        }
    }

    public interface IScreenView<TParam1, TParam2> : IScreenBody
    {
        TParam1 Param1 { get; set; }
        TParam2 Param2 { get; set; }
    }
}