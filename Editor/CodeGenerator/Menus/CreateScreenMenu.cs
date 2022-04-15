namespace MVC.Editor.CodeGenerator.Menus
{
    internal class CreateScreenMenu : CreateViewMenu
    {
        protected override string _classLabelName => "Screen Name: ";
        protected override string _classViewName => "ScreenView";
        protected override string _classMediatorName => "ScreenMediator";

        protected override string _namespace => "Runtime.Views.Screens.";

        protected override string _tempViewName => "TempScreenView";
        protected override string _tempMediatorName => "TempScreenMediator";

        protected override string _targetViewPath => CodeGeneratorStrings.ScreenPath;
        protected override string _tempViewPath => CodeGeneratorStrings.TempScreenViewPath;
        protected override string _tempMediatorPath => CodeGeneratorStrings.TempScreenMediatorPath;
    }
}