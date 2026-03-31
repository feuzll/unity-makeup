#nullable enable
using System.Collections.Generic;
using abc.DressUp.Interactions;
using abc.DressUp.Model;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.View
{
    public class Room : MonoBehaviour, IRoom
    {
        [SerializeField] private IHand? handHandScope;
        [SerializeField] private Dictionary<ITool, IRoom.HandToolReadyPlace> tools;
        [SerializeField] private IFaceZone faceZone;

        Dictionary<ITool, IRoom.HandToolReadyPlace> IRoom.AvailableTools => tools;
        IFaceZone IRoom.FaceZone => faceZone;

        IHand IRoom.Hand => handHandScope;
    }
}