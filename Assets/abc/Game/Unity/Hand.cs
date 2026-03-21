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
        [SerializeField] private Canvas canvas;
        [SerializeField] private float followDuration = 0.12f;
        [SerializeField] private Ease followEase = Ease.OutQuad;

        private RectTransform _rectTransform;
        private PrimeTween.Tween _moveTween;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void IHand.RequestMoveTo(float x, float y)
        {
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
    }
}