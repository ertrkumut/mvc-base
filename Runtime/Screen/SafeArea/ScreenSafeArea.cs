using System.Collections.Generic;
using MVC.Runtime.Screen.Layer;
using UnityEngine;

namespace MVC.Runtime.Screen.SafeArea
{
    public class ScreenSafeArea : MonoBehaviour
    {
        private List<RectTransform> _layerRects = new ();
        private ScreenOrientation _lastOrientation;
        private Vector2 _lastResolution = Vector2.zero;
        private Rect _lastSafeArea = Rect.zero;
        void Awake()
        {
            foreach (var layer in transform.GetComponentsInChildren<ScreenLayer>())
            {
                if (layer.IsSafeAreaExists)
                    _layerRects.Add(layer.GetComponent<RectTransform>());
            }

            SetVariables();

            ApplySafeArea();
        }

        private void SetVariables()
        {
            _lastOrientation = UnityEngine.Screen.orientation;
            _lastResolution.x = UnityEngine.Screen.width;
            _lastResolution.y = UnityEngine.Screen.height;
            _lastSafeArea = UnityEngine.Screen.safeArea;
        }

        void Update()
        {
            if (Application.isMobilePlatform && UnityEngine.Screen.orientation != _lastOrientation) OrientationChanged();

            if (UnityEngine.Screen.width != _lastResolution.x || UnityEngine.Screen.height != _lastResolution.y) ResolutionChanged();

            if (UnityEngine.Screen.safeArea != _lastSafeArea) SafeAreaChanged();
        }

        void ApplySafeArea()
        {
            var anchorMin = _lastSafeArea.position;
            var anchorMax = _lastSafeArea.position + _lastSafeArea.size;
            anchorMin.x /= UnityEngine.Screen.currentResolution.width;
            anchorMin.y /= UnityEngine.Screen.currentResolution.height;
            anchorMax.x /= UnityEngine.Screen.currentResolution.width;
            anchorMax.y /= UnityEngine.Screen.currentResolution.height;
            
            foreach (var rect in _layerRects)
            {
                rect.anchorMin = anchorMin;
                rect.anchorMax = anchorMax;
            }
        }
        private void SafeAreaChanged()
        {
            Debug.Log("Safe Area changed from " + _lastSafeArea + " to " + UnityEngine.Screen.safeArea + " at " + Time.time);

            _lastSafeArea = UnityEngine.Screen.safeArea;
            
            ApplySafeArea();
        }

        private void OrientationChanged()
        {
            Debug.Log("Orientation changed from " + _lastOrientation + " to " + UnityEngine.Screen.orientation + " at " + Time.time);

            _lastOrientation = UnityEngine.Screen.orientation;
            _lastResolution.x = UnityEngine.Screen.width;
            _lastResolution.y = UnityEngine.Screen.height;
        }

        private void ResolutionChanged()
        {
            Debug.Log("Resolution changed from " + _lastResolution + " to (" + UnityEngine.Screen.width + ", " + UnityEngine.Screen.height + ") at " + Time.time);

            _lastResolution.x = UnityEngine.Screen.width;
            _lastResolution.y = UnityEngine.Screen.height;
        }
    }
}