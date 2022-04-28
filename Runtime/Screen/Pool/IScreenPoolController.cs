using MVC.Runtime.Screen.View;

namespace MVC.Runtime.Screen.Pool
{
    public interface IScreenPoolController
    {
        IScreenBody GetScreenFromPool(System.Enum screenType);
        void SendScreenToPool(IScreenBody screenBody);
        
        void RemoveScreenFromPoolDict(IScreenBody screenBody);
        void AddScreenToPoolDict(IScreenBody screenBody);
        
        void RemoveScreenFromActiveDict(IScreenBody screenBody);
        void AddScreenToActiveDict(IScreenBody screenBody);
    }
}