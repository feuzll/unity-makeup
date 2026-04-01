#nullable enable
using System;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public interface IFaceZone
    {
        public event Action? OnInteract;
        public Canvas ParentCanvas { get; } 
        public RectTransform Rect { get; }
    }
}