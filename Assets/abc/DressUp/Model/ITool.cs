#nullable enable
using System;
using UnityEngine;

namespace abc.DressUp.Model
{
    public partial interface ITool
    {
        public Canvas ScreenCanvas { get; }
        public ITool.IContainer InitialContainer { get; }
        public IContainer Container { get; protected set; }
        public Vector2 LocalContainerPosition { get; }
        public event Action? OnInteract;
    }
}