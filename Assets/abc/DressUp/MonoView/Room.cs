#nullable enable
using System;
using System.Collections.Generic;
using abc.DressUp.Interactions;
using abc.DressUp.Entities;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class Room : MonoBehaviour, IRoom
    {
        [SerializeReference] private Hand? hand;
        [SerializeField] private Dictionary<ITool, IRoom.HandToolReadyPlace> tools;
        [SerializeField] private IFaceZone faceZone;
        [SerializeField] private List<BrushPage>  brushPages = new();
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
            }
        }
    }
}