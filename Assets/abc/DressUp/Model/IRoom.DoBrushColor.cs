using System;
using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Model
{
    public partial interface IRoom
    {
        public Sequence DoBrushColor(Interaction.ExecutionToken token,
            IBrush brush, IHand hand)
        {
            if (brush.PendingColorSource is null)
                throw new ArgumentNullException();
            var colorSourceWorld = brush.PendingColorSource!.Rect.position;
            var cam           = brush.ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : brush.ScreenCanvas.worldCamera;
            var colorSourceScreenXY=
                RectTransformUtility.WorldToScreenPoint(cam, colorSourceWorld);
            
            brush.BindTo(token, PendingBrushColor);
            return Sequence.Create(hand.DoBusyMoveToScreenXY(token, colorSourceScreenXY))
                .ChainCallback(() => brush.Color(token));
        }
    }
}