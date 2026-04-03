using System;
using abc.DressUp.Interactions;
using abc.DressUp.Entities;
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

        IBrush.IColorSource IBrush.PendingColorSource { get; set; }

        public ShakeSettings ColorShakeSettings => colorShakeSettings;

        public void Color(Interaction.ExecutionToken token)
        {
            if (((IBrush)this).PendingColorSource == null) throw new ArgumentNullException();
            colorOverlay.color = ((IBrush)this).PendingColorSource!.Color;
            targetCharacterView = ((IBrush)this).PendingColorSource!.ViewState;
        }
    }
}