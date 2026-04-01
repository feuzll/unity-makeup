using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public partial interface IHand
    {
        public Tween DoBusyMoveToScreenXY(
            Interaction.ExecutionToken token, Vector2 screenPoint)
        {
            IsBusy = true;
            
            DragTween?.Stop();
            
            var cam = ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : ScreenCanvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                ParentRect, screenPoint, cam, out var targetAsLocal);
            
            //Debug.Log($"hand moves locally from:{_rectTransform.anchoredPosition}  to:{targetAsLocal}");
            var duration = Vector2.Distance(Rect.anchoredPosition, targetAsLocal) / MoveSpeed;
            return Tween.UIAnchoredPosition(Rect, targetAsLocal, duration, DragEase);
        }
    }
}