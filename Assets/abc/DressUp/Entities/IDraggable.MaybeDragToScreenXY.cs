using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public partial interface IDraggable
    {
        protected Canvas  ScreenCanvas { get; }
        protected Tween? DragTween { get; }
        protected void SetDragTween(Tween tween);
        protected Ease DragEase { get; }
        protected bool CanDrag { get; }
        protected RectTransform ParentRect { get; }
        protected RectTransform Rect { get; }
        
        protected float DragSpeed { get; }
        
        public void MaybeDragToScreenXY(Interaction.ExecutionToken token, Vector2 screenPoint)
        {
            if (!CanDrag) return;
            DragTween?.Stop();
            
            var cam = ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : ScreenCanvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                ParentRect, screenPoint, cam, out var targetAsLocal);
            
            //Debug.Log($"hand moves locally from:{_rectTransform.anchoredPosition}  to:{targetAsLocal}");
            var duration = Vector2.Distance(Rect.anchoredPosition, targetAsLocal) / DragSpeed;
            var motion = Tween.UIAnchoredPosition(Rect, targetAsLocal, duration, DragEase);
            SetDragTween(motion);
        }
    }
}