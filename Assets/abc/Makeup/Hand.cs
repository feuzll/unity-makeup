#nullable enable
using PrimeTween;
using UnityEngine;

namespace abc.Makeup
{
    public partial class Hand : Tool.IContainer
    {
        private readonly float _moveSpeed;
        private readonly RectTransform _rectTransform;
        private Tween _moveTween;
        private RectTransform _parentRect;
        public bool IsBusy { get; set; } = false;
        private Canvas _canvas;
        private Ease _followEase;
        private Vector3 _worldRestPosition;

        public RectTransform Rect => _rectTransform;
        
        public Hand(float moveSpeed, RectTransform rectTransform, Canvas canvas, Ease followEase)
        {
            _moveSpeed = moveSpeed;
            _rectTransform = rectTransform;
            _canvas = canvas;
            _followEase = followEase;
            _parentRect = (RectTransform)_rectTransform.parent;
            _worldRestPosition = _parentRect.TransformPoint(rectTransform.anchoredPosition);
        }
        
        Tool? Tool.IContainer.HeldTool { get; set; }

        public Vector3 WorldRestPosition => _worldRestPosition;
    }
}