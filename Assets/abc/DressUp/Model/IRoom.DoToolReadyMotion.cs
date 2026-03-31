using System;
using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Model
{
    public partial interface IRoom
    {
        private Vector2 GetReadyWorld(ITool tool)
        {
            if (AvailableTools.TryGetValue(tool, out var finalPlaceType))
                throw new ArgumentException(tool.ToString());
            
            Vector2 readyWorld;
            var faceWorld = 
                FaceZone.Rect.TransformPoint(FaceZone.Rect.rect.center);
            
            switch (finalPlaceType)
            {
                case HandToolReadyPlace.BetweenFaceAndSlot:
                    var slotWorld = 
                        tool.InitialContainer.Rect.TransformPoint(tool.InitialContainer.ToolLocalBindPosition);
                    readyWorld = Vector3.Lerp(slotWorld, faceWorld, 0.5f);
                    break;
                case HandToolReadyPlace.BetweenFaceAndHandRest:
                    readyWorld = Vector3.Lerp(Hand.WorldRestPosition, faceWorld, 0.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return readyWorld;
        }
        
        public Sequence DoToolReadyMotion(Interaction.ExecutionToken token,
            ITool tool, IHand hand)
        {
            var readyWorld = GetReadyWorld(tool);
            
            return Sequence.Create().Chain(hand.DoBusyMoveToScreenXY(token, readyWorld));
        }

        public Sequence DoToolReadyMotion(Interaction.ExecutionToken token,
            IBrush brush, IHand hand, RectTransform colorSource)
        {
            if (AvailableTools.TryGetValue(brush, out var finalPlaceType))
                throw new ArgumentException(brush.ToString());
            
            var readyWorld = GetReadyWorld(brush);
            
            return Sequence.Create().Chain(DoBrushColor(token, brush, hand, colorSource))
                .Chain(hand.DoBusyMoveToScreenXY(token, readyWorld));
        }
    }
}