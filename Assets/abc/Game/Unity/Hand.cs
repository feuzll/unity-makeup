using System;
using abc.Game.Model;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public partial class Hand : MonoBehaviour, IHand
    {
        Action IHand.ActiveToolChanged => _activeToolChanged;
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private float followDuration = 0.12f;
        [SerializeField] private Ease followEase = Ease.OutQuad;

        private RectTransform _rectTransform;
        private PrimeTween.Tween _moveTween;
        private Vector2 _startPosition;
        private IHand.ITool _tool;
        private readonly Action _activeToolChanged;
        
        IHand.ITool IHand.Tool
        {
            get => _tool;
            set => _tool = value;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.anchoredPosition;
        }

        void IHand.TryMoveTo(float x, float y)
        {
            if (_tool is not null) return;
            var parentRect = (RectTransform)_rectTransform.parent;
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect, new Vector2(x, y), cam, out var localPoint)) return;
            
            _moveTween.Stop();
            _moveTween = PrimeTween.Tween.UIAnchoredPosition(
                _rectTransform,
                localPoint,
                followDuration,
                followEase);
        }

        void IHand.TryDrop(IHand.ITool tool)
        {
            if (_tool is null) return;
            //dropping animation
            _tool = null;
            _activeToolChanged?.Invoke();
        }
    }
}