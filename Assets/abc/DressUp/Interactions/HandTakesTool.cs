using abc.DressUp.Model;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Interactions
{
    public class HandTakesTool : Interaction
    {
        public HandTakesTool(IHand hand, ITool tool, 
            Sequence readyMotion, out Sequence? sequence)
        {
            sequence = null;
            if (hand.IsBusy) return;
            sequence = Sequence.Create()
                .Chain(hand.DoBusyMoveToScreenXY(Token, Vector2.zero))
                .ChainCallback(() => hand.TakeTool(Token, tool))
                .Chain(readyMotion)
                .ChainCallback(() => hand.Unbusy(Token));
        }
    }
}