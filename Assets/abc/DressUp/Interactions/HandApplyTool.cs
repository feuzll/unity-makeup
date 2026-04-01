using abc.DressUp.Model;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Interactions
{
    public class HandApplyTool : Interaction
    {
        public HandApplyTool(IHand hand, ITool tool, IFaceZone face, ICharacter character)
        {
            var faceWorld = face.Rect.position;
            var cam = face.ParentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                null : face.ParentCanvas.worldCamera;
            var faceScreenXY = RectTransformUtility.WorldToScreenPoint(cam, faceWorld);
            
            var toolReturnWorld =
                tool.InitialContainer.Rect.TransformPoint(tool.InitialContainer.ToolLocalBindPosition);
            cam = tool.ScreenCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                null : tool.ScreenCanvas.worldCamera;
            var toolReturnScreenXY =  RectTransformUtility.WorldToScreenPoint(cam, toolReturnWorld);
            
            
            Sequence.Create()
                .Chain(hand.DoBusyMoveToScreenXY(Token, faceScreenXY))
                .Chain(Tween.ShakeLocalPosition(hand.Rect, tool.ApplySettings))
                .ChainCallback(target: this, target => 
                    character.ApplyViewState(Token, tool.TargetCharacterView))
                .Chain(hand.DoBusyMoveToScreenXY(Token, toolReturnScreenXY))
                .ChainCallback(() => tool.InitialContainer.TakeTool(Token, tool))
                .Chain(hand.DoReturnToRest(Token));
        }
    }
}