using UnityEngine;

namespace abc.Makeup
{
    public partial class Hand
    {
        public Vector2 WorldToLocal(Vector3 worldPosition)
        {
            var cam           = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : _canvas.worldCamera;
            var screenPos     = RectTransformUtility.WorldToScreenPoint(cam, worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRect, screenPos, cam, out var targetAsLocal);
            return targetAsLocal;
        }
    }
}