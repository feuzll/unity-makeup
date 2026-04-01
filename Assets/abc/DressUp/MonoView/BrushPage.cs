using System;
using System.Collections.Generic;
using abc.DressUp.Entities;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class BrushPage : MonoBehaviour, IBrush.IPage
    {
        [SerializeField] private IBrush brush;
        [SerializeField] private List<IBrush.IColorSource> colorSources;

        public event Action<IBrush, IBrush.IColorSource> OnDecidedBrushUse;
        
        private void Awake()
        {
            foreach (var colorSource in colorSources)
            {
                colorSource.Clicked += () =>
                {
                    OnDecidedBrushUse?.Invoke(brush, colorSource);
                    (this as IBrush.IPage).RaiseToolInteract(brush);
                };
            }
        }
    }
}