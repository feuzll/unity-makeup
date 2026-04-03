#nullable enable
using System;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public partial interface ITool
    {
        public Canvas ScreenCanvas { get; }
        public Vector2 HandGripOffset { get; }
        public Vector2 ApplyPointOffset { get; }
        public ITool.IContainer InitialContainer { get; }
        public IContainer Container { get; protected set; }
        public Vector2 LocalInitialContainerPosition { get; }
        public ShakeSettings ApplySettings { get; }
        public ICharacter.ViewState TargetCharacterView { get; }
        public event Action? OnInteract;
        protected void RaiseOnInteract();
    }
}