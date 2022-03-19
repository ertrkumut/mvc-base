using MVC.Examples.Controller;
using MVC.Examples.Entity;
using MVC.Examples.Models;
using MVC.Examples.Signals;
using MVC.Examples.Views.Player;
using MVC.Runtime.Contexts;

namespace MVC.Examples.Contexts
{
    public class GameContext : Context
    {
        private GameSignals _gameSignals;
        
        public override void MapBindings()
        {
            base.MapBindings();

            CrossContextInjectionBinder.Bind<TestClass>();
            CrossContextInjectionBinder.Bind<TestClass>("Test");
            CrossContextInjectionBinder.Bind<TestClass>("Test2");
            // InjectionBinder.Bind<TestClass>();

            _gameSignals = CrossContextInjectionBinder.Bind<GameSignals>();
            
            CommandBinder.Bind(_gameSignals.Start)
                .To<TestCommand1>()
                .To<TestCommand2>();
            
            CommandBinder.Bind(_gameSignals.IntTestSignal)
                .To<TestCommand1>()
                .To<TestCommand2>();

            CommandBinder.Bind(_gameSignals.IntTest2Signal)
                .To<TestCommand2>()
                .To<TestCommand3>();
            
            BindViews();
            BindModels();
        }

        private void BindViews()
        {
            MediatorBinder.Bind<PlayerControllerView>().To<PlayerControllerMediator>();
        }
        
        private void BindModels()
        {
            CrossContextInjectionBinder.Bind<ITestModel, TestModel>();
        }

        public override void Launch()
        {
            base.Launch();
            
            // _gameSignals.Start.Dispatch();
            _gameSignals.IntTestSignal.Dispatch(10);
            // _gameSignals.IntTest2Signal.Dispatch(25);
        }
    }
}