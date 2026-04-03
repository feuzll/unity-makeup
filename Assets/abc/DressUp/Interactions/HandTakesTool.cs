#nullable enable
using System;
using abc.DressUp.Entities;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Interactions
{
    public class HandTakesTool : Interaction
    {
        public HandTakesTool(IHand hand, ITool tool, 
            IToolReadyMotionBuilder readyMotionBuilder, Action? callback = null)
        {
            if (hand.IsBusy) return;

            var toolWorld = tool.Container.Rect.TransformPoint(tool.LocalInitialContainerPosition);
            var cam = tool.ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                null : tool.ScreenCanvas.worldCamera;
            var toolScreen = RectTransformUtility.WorldToScreenPoint(null, toolWorld);
            
            Sequence.Create()
                .Chain(hand.DoBusyMoveToScreenXY(Token, toolScreen + tool.HandGripOffset))
                .ChainCallback(() => hand.TakeTool(Token, tool))
                .Chain(readyMotionBuilder.BuildAndRunFor(Token, tool))
                .ChainCallback(() => hand.Unbusy(Token))
                .ChainCallback(callback);
        }
    }
}