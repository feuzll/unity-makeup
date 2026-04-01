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

        Sequence IToolReadyMotionBuilder.BuildAndRunFor(Interaction.ExecutionToken token, ITool tool)
        {
            var readyWorld = GetReadyWorld(tool);
            
            if (tool is IBrush brush)
                return Sequence.Create().Chain(DoBrushColor(token, brush, Hand))
                    .Chain(Hand.DoBusyMoveToScreenXY(token, readyWorld));
            else
                return Sequence.Create().Chain(Hand.DoBusyMoveToScreenXY(token, readyWorld));
        }
    }
}