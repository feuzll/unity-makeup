#nullable enable
using System;
using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public partial interface IBrush : ITool
    {
        public IColorSource? PendingColorSource { get; protected set; }
        public ShakeSettings ColorShakeSettings { get; }
        public void Color(Interaction.ExecutionToken token);
        public void BindTo(Interaction.ExecutionToken token, IColorSource colorSource) 
            => PendingColorSource = colorSource;
        public event Action<Color>? ColorChanged;
        
        public interface IColorSource
        {
            public Color Color { get; }
            public RectTransform Rect { get; }
            public ICharacter.ViewState ViewState { get; }
            public event Action? Clicked;
        }
    }
}