#nullable enable
using System;
using abc.Makeup.MonoBuilder;
using PrimeTween;
using UnityEngine;

namespace abc.Makeup
{
    public partial class Room
    {
        public Tool[] AvailableTools { get; }

        private Hand _hand;
        private FaceZone _faceZone;
        
        public Room(Tool[] availableTools, Hand hand, FaceZone faceZone)
        {
            AvailableTools = availableTools;
            _hand = hand;
            _faceZone = faceZone;
            foreach (var tool in availableTools)
            {
                tool.Interactable.raycastTarget = true;
                tool.OnClick += () => 
                {
                    Debug.Log($"tool reacts to click");
                    TryTakeTool(tool);
                };
                faceZone.OnInteract += () => TryToApplyTool(tool);
            }
        }

        private void TryToApplyTool(Tool tool)
        {
            if (_hand.IsBusy) return;
            Debug.Log($"applying tool {tool}");
            DisableAllTools();
            _hand.IsBusy = true;
            Sequence.Create() 
                .Chain(MoveHandToFaceStep())
                .Chain(Tween.ShakeLocalPosition(_hand.Rect, tool.ApplySettings))
                .Chain(_hand.ReturnTool(tool))
                .Chain(_hand.ReturnToRest())
                .ChainCallback(() =>
                {
                    tool.Interactable.raycastTarget = true;
                    _hand.IsBusy = false;
                    EnableAllTools();
                });
            
        }

        private void TryTakeTool(Tool tool)
        {
            if (_hand.IsBusy) return;
            Debug.Log($"tool from loop is {tool}");
            DisableAllTools();
            _hand.IsBusy = true;
            tool.Interactable.raycastTarget = false;
            _hand.PerformToolTake(tool)
                .Chain(ReadyHandWithToolStep(tool))
                .ChainCallback(() =>
                {
                    _hand.IsBusy = false;
                    EnableAllTools();
                });
            
        }

        private void EnableAllTools() {
            foreach (var tool in AvailableTools) tool.Interactable.raycastTarget = true;
        }

        private void DisableAllTools() {
            foreach (var tool in AvailableTools) tool.Interactable.raycastTarget = false;
        }
        
        private Tween ReadyHandWithToolStep(Tool tool)
        {
            var faceWorld = tool.Rect.TransformPoint(_faceZone.Rect.rect.center);
            Vector2 readyWorld;
            
            switch (tool.HandReady)
            {
                case Tool.HandReadyType.BetweenFaceAndSlot:
                    var slotWorld = 
                        tool.Rect.TransformPoint(tool.InitialContainer.Rect.rect.center);
                    readyWorld = Vector3.Lerp(slotWorld, faceWorld, 0.5f);
                    break;
                case Tool.HandReadyType.BetweenFaceAndHandRest:
                    readyWorld = Vector3.Lerp(_hand.WorldRestPosition, faceWorld, 0.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return _hand.MoveToWorld(readyWorld);
        }

        private Tween MoveHandToFaceStep()
        {
            var faceApplyWorld = 
                _faceZone.Rect.TransformPoint(_faceZone.Rect.rect.center);
            return _hand.MoveToWorld(faceApplyWorld);
        }
    }
}