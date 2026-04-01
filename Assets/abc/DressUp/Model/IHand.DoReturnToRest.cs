using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Model
{
    public partial interface IHand
    {
        public Sequence DoReturnToRest(Interaction.ExecutionToken token)
        {
            var cam = ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                null : ScreenCanvas.worldCamera;
            var restScreenXY =  RectTransformUtility.WorldToScreenPoint(cam, WorldRestPosition);
            return Sequence.Create()
                .Chain(DoBusyMoveToScreenXY(token, restScreenXY))
                .ChainCallback(target: this, target => Unbusy(token));
        }
    }
}