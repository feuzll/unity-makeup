#nullable enable
using PrimeTween;
using UnityEngine;

namespace abc.Makeup
{
    public partial class Hand
    {
        private void TryDragToScreenPoint(Vector2 screenPoint)
        {
            if ((this as Tool.IContainer).HeldTool is null) return;
            if (IsBusy) return;
            _moveTween.Stop();
            
            var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : _canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRect, screenPoint, cam, out var targetAsLocal);
            
            //Debug.Log($"hand moves locally from:{_rectTransform.anchoredPosition}  to:{targetAsLocal}");
            var duration = Vector2.Distance(_rectTransform.anchoredPosition, targetAsLocal) / _moveSpeed;
            _moveTween = Tween.UIAnchoredPosition(_rectTransform, targetAsLocal, duration, _followEase);
        }
    }
}