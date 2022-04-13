namespace MVC.Screen.View
{
    public class ScreenView<TParam1, TParam2, TParam3> : ScreenBody, IScreenView<TParam1, TParam2, TParam3>
    {
        public TParam1 Param1 { get; set; }
        public TParam2 Param2 { get; set; }
        public TParam3 Param3 { get; set; }

        internal override void InitializeScreenParams(params object[] screenParams)
        {
            Param1 = (TParam1) screenParams[0];
            Param2 = (TParam2) screenParams[1];
            Param3 = (TParam3) screenParams[2];
        }
    }

    public interface IScreenView<TParam1, TParam2, TParam3> : IScreenBody
    {
        TParam1 Param1 { get; set; }
        TParam2 Param2 { get; set; }
        TParam3 Param3 { get; set; }
    }
}