#nullable enable
using System;
using abc.DressUp.Entities;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Interactions
{
    public class HandApplyTool : Interaction
    {
        public HandApplyTool(IHand hand, IFaceZone face, ICharacter character, Action? callback = null)
        {
            if (hand.IsBusy) return;
            if (hand.HeldTool is null) return;
            
            var tool = hand.HeldTool;
            
            var faceWorld = face.Rect.position;
            var cam = face.ParentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                null : face.ParentCanvas.worldCamera;
            var faceScreenXY = RectTransformUtility.WorldToScreenPoint(cam, faceWorld);
            
            var toolReturnWorld =
                tool.InitialContainer.Rect.TransformPoint(tool.LocalInitialContainerPosition);
            cam = tool.ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                null : tool.ScreenCanvas.worldCamera;
            var toolReturnScreenXY =  RectTransformUtility.WorldToScreenPoint(cam, toolReturnWorld);
            
            
            Sequence.Create()
                .Chain(hand.DoBusyMoveToScreenXY(Token, faceScreenXY + tool.ApplyPointOffset))
                .Chain(Tween.ShakeLocalPosition(hand.Rect, tool.ApplySettings))
                .ChainCallback(target: this, target => 
                    character.ApplyViewState(Token, tool.TargetCharacterView))
                .Chain(hand.DoBusyMoveToScreenXY(Token, toolReturnScreenXY + tool.HandGripOffset))
                .ChainCallback(() =>
                {
                    tool.DisableSorting(Token);
                    tool.InitialContainer.TakeTool(Token, tool);
                })
                .Chain(hand.DoReturnToRest(Token))
                .ChainCallback(target:this, target => callback?.Invoke());
        }
    }
}