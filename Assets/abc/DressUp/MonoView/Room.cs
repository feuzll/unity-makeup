#nullable enable
using System;
using System.Collections.Generic;
using abc.DressUp.Interactions;
using abc.DressUp.Entities;
using PrimeTween;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class Room : SerializedMonoBehaviour, IRoom
    {
        [SerializeReference] private Hand? hand;
        [OdinSerialize] private Dictionary<ITool, IRoom.HandToolReadyPlace> tools;
        [SerializeField] private IFaceZone faceZone;
        [SerializeField] private List<BrushPage>  brushPages = new();
        [SerializeField] ICharacter character;
        private IBrush.IColorSource _pendingBrushBrushColor;

        Dictionary<ITool, IRoom.HandToolReadyPlace> IRoom.AvailableTools => tools;
        IFaceZone IRoom.FaceZone => faceZone;

        IHand IRoom.Hand => hand;

        IBrush.IColorSource IRoom.PendingBrushColor
        {
            get => _pendingBrushBrushColor;
            set => _pendingBrushBrushColor = value;
        }

        private void Awake()
        {
            foreach (var brushPage in brushPages)
            {
                brushPage.OnDecidedBrushUse += (brush, source) =>
                {
                    _pendingBrushBrushColor = source;
                };
            }

            foreach (var tool in tools.Keys)
            {
                tool.OnInteract += () =>
                {
                    new HandTakesTool(hand, tool, this, out _);
                };

                faceZone.OnInteract += () =>
                {
                    new HandApplyTool(hand, tool, faceZone, character);
                };
            }
        }
    }
}