#nullable enable
using System;
using abc.DressUp.Interactions;
using abc.DressUp.Entities;
using JetBrains.Annotations;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace abc.DressUp.MonoView
{
    public class Brush : Tool, IBrush
    {
        [SerializeField] private Image colorOverlay;
        [SerializeField] private BrushPage brushPage;
        [SerializeField] private ShakeSettings colorShakeSettings;
        [SerializeField] private Vector2 handColorMoveOffset;

        IBrush.IColorSource? IBrush.PendingColorSource { get; set; }

        public ShakeSettings ColorShakeSettings => colorShakeSettings;

        public Vector2 BrushTipOffset => handColorMoveOffset;

        public void Color(Interaction.ExecutionToken token)
        {
            if (((IBrush)this).PendingColorSource == null) throw new ArgumentNullException();
            colorOverlay.color = ((IBrush)this).PendingColorSource!.Color;
            targetCharacterView = ((IBrush)this).PendingColorSource!.ViewState;
            ColorChanged?.Invoke(colorOverlay.color);
        }

        public event Action<Color>? ColorChanged;
    }
}