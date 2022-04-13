using System.Collections.Generic;
using MVC.Runtime.Injectable.Attributes;

namespace MVC.Screen
{
    public class ScreenModel : IScreenModel
    {
        private Dictionary<int, IScreenManager> _screenManagerDict;

        [PostConstruct]
        protected void PostConstruct()
        {
            _screenManagerDict = new Dictionary<int, IScreenManager>();
        }

        public void RegisterScreenManager(ScreenManager screenManager)
        {
            if(!_screenManagerDict.ContainsKey(screenManager.ManagerIndex))
                _screenManagerDict.Add(screenManager.ManagerIndex, screenManager);
        }

        public void UnRegisterScreenManager(ScreenManager screenManager)
        {
            if(_screenManagerDict.ContainsKey(screenManager.ManagerIndex))
                _screenManagerDict.Remove(screenManager.ManagerIndex);
        }
    }

    public interface IScreenModel
    {
        void RegisterScreenManager(ScreenManager screenManager);
        void UnRegisterScreenManager(ScreenManager screenManager);
    }
}