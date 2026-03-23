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
        [SerializeField] private float   _returnToRestDuration = 0.3f;
        
        [Header("Scripted movement")]
        [SerializeField] private float _moveSpeed       = 800f; // px/sec, scripted sequences
        [SerializeField] private float _returnToRestSpeed = 600f;

        public float MoveSpeed        => _moveSpeed;
        public float ReturnToRestSpeed => _returnToRestSpeed;
        
        public Model.Hand Data { get; } = new();

        private RectTransform _rectTransform;
        private Vector2 _restAnchoredPosition;
        private PrimeTween.Tween  _moveTween;

        private void Awake()
        {
            _rectTransform       = GetComponent<RectTransform>();
            //_rectTransform.pivot = new Vector2(0.5f, 1f);
            _restAnchoredPosition =  _rectTransform.anchoredPosition;

            // React to model changes — all rendering lives here
            Data.PositionChanged += OnPositionChanged;
        }
        
        private void OnDestroy() =>
            Data.PositionChanged -= OnPositionChanged;

        private void OnPositionChanged()
        {
            if (!ScreenToLocal(
                    new Vector2(Data.Position.x, Data.Position.y),
                    out var localPoint)) return;

            _moveTween.Stop();
            _moveTween = Tween.UIAnchoredPosition(
                _rectTransform, localPoint, _followDuration, _followEase);
        }

        public Tween TweenToAnchored(Vector2 to)
        {
            _moveTween.Stop();
            var duration = Vector2.Distance(_rectTransform.anchoredPosition, to) / _moveSpeed;
            return Tween.UIAnchoredPosition(_rectTransform, to, duration);
        }

        public Tween TweenToAnchored(Vector2 from, Vector2 to)
        {
            var duration = Vector2.Distance(from, to) / _moveSpeed;
            return Tween.UIAnchoredPosition(_rectTransform, to, duration);
        }

        public Tween TweenToRest()
        {
            _moveTween.Stop();
            var duration = Vector2.Distance(
                _rectTransform.anchoredPosition, _restAnchoredPosition) / _returnToRestSpeed;
            return Tween.UIAnchoredPosition(_rectTransform, _restAnchoredPosition, duration);
        }
        
        // Called by JarBehaviour for scripted movement
        public PrimeTween.Tween TweenToAnchored(Vector2 target, float duration, Ease ease = Ease.OutQuad)
        {
            _moveTween.Stop();
            return Tween.UIAnchoredPosition(_rectTransform, target, duration, ease);
        }

        public Vector2 AnchoredPosition => _rectTransform.anchoredPosition;
        public Vector2 RestPosition     => _restAnchoredPosition;

        public bool ScreenToLocal(Vector2 screenPoint, out Vector2 localPoint)
        {
            var parentRect = (RectTransform)_rectTransform.parent;
            var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : _canvas.worldCamera;
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect, screenPoint, cam, out localPoint);
        }
    }
}