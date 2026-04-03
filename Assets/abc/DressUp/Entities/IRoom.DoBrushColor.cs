using System;
using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public partial interface IRoom
    {
        public Sequence DoBrushColor(Interaction.ExecutionToken token,
            IBrush brush, IHand hand)
        {
            brush.BindTo(token, PendingBrushColor);
            var colorSourceWorld = brush.PendingColorSource!.Rect.position;
            var cam           = brush.ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : brush.ScreenCanvas.worldCamera;
            var colorSourceScreenXY=
                RectTransformUtility.WorldToScreenPoint(cam, colorSourceWorld);
            
            return Sequence.Create(hand.DoBusyMoveToScreenXY(token, colorSourceScreenXY))
                .Chain(Tween.ShakeLocalPosition(hand.Rect, brush.ColorShakeSettings))
                .ChainCallback(() => brush.Color(token));
        }
    }
}