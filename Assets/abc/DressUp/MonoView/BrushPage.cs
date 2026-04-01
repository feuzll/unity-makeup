using System;
using System.Collections.Generic;
using abc.DressUp.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class BrushPage : SerializedMonoBehaviour, IBrush.IPage
    {
        [SerializeField] private IBrush brush;
        [SerializeField] private List<IBrush.IColorSource> colorSources;

        public event Action<IBrush, IBrush.IColorSource> OnDecidedBrushUse;

        [Button]
        protected void FillColorSources()
        {
            foreach (var child 
                     in transform.GetComponentsInChildren<BrushColorSource>())
                {
                colorSources.Add(child);
                }
        }
        
        private void Awake()
        {
            foreach (var colorSource in colorSources)
            {
                colorSource.Clicked += () =>
                {
                    Debug.Log("inside colorsource clicked event");
                    OnDecidedBrushUse?.Invoke(brush, colorSource);
                    (this as IBrush.IPage).RaiseToolInteract(brush);
                };
            }
        }
    }
}