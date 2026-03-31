#nullable enable
using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace abc.Makeup
{
    public partial class Tool
    {
        public readonly RectTransform Rect;
        public readonly Graphic Interactable;
        private IContainer _container;
        private IContainer _initialContainer;
        public HandReadyType HandReady { get; private set; }
        public ShakeSettings ApplySettings { get; }
        public event Action? OnClick;
        private Vector2 _slotPosition;
        
        public IContainer InitialContainer => _initialContainer;
        public IContainer Container
        {
            get => _container;
            private set
            {
                _container = value;
                Rect.SetParent(_container.Rect, true);
            }
        }

        public Vector2 SlotPosition => _slotPosition;

        public Tool(RectTransform rect, Graphic interactable, IContainer container, HandReadyType handReady,
            ShakeSettings applySettings)
        {
            Rect = rect;
            Interactable = interactable;
            _container = _initialContainer = container;
            HandReady = handReady;
            ApplySettings = applySettings;
            Interactable.raycastTarget = true;
            _slotPosition = rect.anchoredPosition;
        }
        

        public interface IInput
        {
            public void RaiseOnClick(Tool model) => model.OnClick?.Invoke();
        }
    }
}