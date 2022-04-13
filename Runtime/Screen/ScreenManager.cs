using MVC.Screen.Layer;
using UnityEngine;

namespace MVC.Screen
{
    public class ScreenManager : MonoBehaviour, IScreenManager
    {
        [SerializeField] private int _managerIndex;
        public int ManagerIndex => _managerIndex;

        [SerializeField] private ScreenLayer[] _screenLayerList;
        public ScreenLayer[] ScreenLayerList => _screenLayerList;

        public void ShowScreen()
        {
            
        }
    }

    public interface IScreenManager
    {
        int ManagerIndex { get; }
        
        ScreenLayer[] ScreenLayerList { get; }
    }
}