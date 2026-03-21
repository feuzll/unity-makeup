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
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float  _followDuration = 0.12f;
        [SerializeField] private Ease   _followEase     = Ease.OutQuad;

        public Model.Hand Data { get; } = new();

        private RectTransform _rectTransform;
        private PrimeTween.Tween  _moveTween;

        private void Awake()
        {
            _rectTransform       = GetComponent<RectTransform>();
            _rectTransform.pivot = new Vector2(0.5f, 1f);

            // React to model changes — all rendering lives here
            Data.PositionChanged += OnPositionChanged;
        }

        private void OnDestroy() =>
            Data.PositionChanged -= OnPositionChanged;

        private void OnPositionChanged()
        {
            var parentRect = (RectTransform)_rectTransform.parent;
            var cam        = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : _canvas.worldCamera;

            var screenPoint = new Vector2(Data.Position.x, Data.Position.y);
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect, screenPoint, cam, out var localPoint)) return;

            _moveTween.Stop();
            _moveTween = PrimeTween.Tween.UIAnchoredPosition(
                _rectTransform, localPoint, _followDuration, _followEase);
        }
    }
}