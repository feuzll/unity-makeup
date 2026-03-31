using PrimeTween;
using UnityEngine;

namespace abc.Makeup
{
    public partial class Hand
    {
        private Tween MoveToTarget(RectTransform target)
        {
            _moveTween.Stop();
            var cam           = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : _canvas.worldCamera;
            var targetWorld   = target.parent.TransformPoint(target.anchoredPosition);
            var screenPos     = RectTransformUtility.WorldToScreenPoint(cam, targetWorld);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRect, screenPos, cam, out var targetAsLocal);
            
            var duration = Vector2.Distance(_rectTransform.anchoredPosition, targetAsLocal) / _moveSpeed;
            return Tween.UIAnchoredPosition(_rectTransform, targetAsLocal, duration);
        }
    }
}