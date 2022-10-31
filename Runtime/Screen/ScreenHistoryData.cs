using MVC.Runtime.Attributes;
using MVC.Runtime.Screen.Enum;

namespace MVC.Runtime.Screen
{
    internal class ScreenHistoryData
    {
        [ReadOnly] public int ManagerIndex;
        [ReadOnly] public System.Enum ScreenType;
        [ReadOnly] public ScreenLayerIndex LayerIndex;

        public ScreenHistoryData(int managerIndex, System.Enum screenType, ScreenLayerIndex layerIndex)
        {
            ManagerIndex = managerIndex;
            ScreenType = screenType;
            LayerIndex = layerIndex;
        }
    }
}