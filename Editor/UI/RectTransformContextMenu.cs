#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MVC.Editor.UI
{
    public class RectTransformContextMenu
    {
        [MenuItem("CONTEXT/RectTransform/Set Anchors To Rect", false, 151)]
        static void SetEasyAnchors()
        {
            var objs = Selection.gameObjects;

            foreach (var o in objs)
            {
                if (o != null && o.GetComponent<RectTransform>() != null)
                {
                    var r = o.GetComponent<RectTransform>();
                    var p = o.transform.parent.GetComponent<RectTransform>();

                    var offsetMin = r.offsetMin;
                    var offsetMax = r.offsetMax;
                    var _anchorMin = r.anchorMin;
                    var _anchorMax = r.anchorMax;

                    var parent_width = p.rect.width;
                    var parent_height = p.rect.height;

                    var anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
                        _anchorMin.y + (offsetMin.y / parent_height));
                    var anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
                        _anchorMax.y + (offsetMax.y / parent_height));

                    r.anchorMin = anchorMin;
                    r.anchorMax = anchorMax;

                    r.offsetMin = new Vector2(0, 0);
                    r.offsetMax = new Vector2(0, 0);
                    r.pivot = new Vector2(0.5f, 0.5f);
                }
            }
        }

        [MenuItem("CONTEXT/RectTransform/Set Anchors To Pivot", false, 151)]
        static void SetAnchorsToPivot()
        {
            var objs = Selection.gameObjects;

            foreach (var o in objs)
            {
                if (o != null && o.GetComponent<RectTransform>() != null)
                {
                    var rectTransform = o.GetComponent<RectTransform>();
                    var parentRectTransform = o.transform.parent.GetComponent<RectTransform>();

                    Rect rect = rectTransform.rect;

                    var offsetMin = rectTransform.offsetMin;
                    var offsetMax = rectTransform.offsetMax;
                    var _anchorMin = rectTransform.anchorMin;
                    var _anchorMax = rectTransform.anchorMax;
                    var sizeDelta = rectTransform.sizeDelta;

                    var parent_width = parentRectTransform.rect.width;
                    var parent_height = parentRectTransform.rect.height;

                    var anchorMin = new Vector2((rectTransform.localPosition.x + parent_width / 2) / parent_width, (rectTransform.localPosition.y + parent_height / 2) / parent_height);
                    var anchorMax = new Vector2((rectTransform.localPosition.x + parent_width / 2) / parent_width, (rectTransform.localPosition.y + parent_height / 2) / parent_height);

                    rectTransform.anchorMin = anchorMin;
                    rectTransform.anchorMax = anchorMax;

                    rectTransform.anchoredPosition = Vector3.zero;
                    rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
                }
            }
        }
    }
}
#endif