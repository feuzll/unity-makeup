using System;
using abc.DressUp.Interactions;
using abc.DressUp.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace abc.DressUp.MonoView
{
    public class Brush : Tool, IBrush
    {
        [SerializeField] private Image colorOverlay;
        [SerializeField] private BrushPage brushPage;
        
        IBrush.IColorSource IBrush.PendingColorSource { get; set; }

        public void Color(Interaction.ExecutionToken token)
        {
            if (((IBrush)this).PendingColorSource == null) throw new ArgumentNullException();
            colorOverlay.color = ((IBrush)this).PendingColorSource!.Color;
            targetCharacterView = ((IBrush)this).PendingColorSource!.ViewState;
        }
    }
}