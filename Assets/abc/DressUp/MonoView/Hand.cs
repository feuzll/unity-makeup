#nullable enable
using System;
using System.Collections;
using abc.DressUp.Entities;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    [RequireComponent(typeof(RectTransform))]
    public class Hand : MonoBehaviour, IHand
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private RectTransform? rectTransform;
        [SerializeField] private RectTransform? parentRect;
        [SerializeField] private Canvas? screenCanvas;
        [SerializeField] private Ease dragEase;
        [SerializeField] private Vector2 toolLocalBindPosition = Vector2.zero;
        private Vector3 _worldRestPosition;
        private bool _canDrag;
        private Tween? _dragTween = null;

        public Vector3 WorldRestPosition => _worldRestPosition;

        private void TryFillReferences()
        {
            if (screenCanvas == null) screenCanvas = GetComponentInParent<Canvas>();
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            if (parentRect == null) parentRect = transform.parent.GetComponent<RectTransform>();
        }

        private void Awake()
        {
            TryFillReferences();
        }

        private IEnumerator Start()
        {
            yield return null; //wait UI to build
            _worldRestPosition = parentRect!.TransformPoint(
                rectTransform!.anchoredPosition);
        }

        private void OnValidate()
        {
            TryFillReferences();
        }

        RectTransform IHand.Rect => rectTransform!;

        float IDraggable.DragSpeed => moveSpeed;

        RectTransform IDraggable.Rect => rectTransform!;
        RectTransform ITool.IContainer.Rect => rectTransform!;

        public ITool? HeldTool { get; private set; }
        

        bool IHand.IsBusy { get; set; } = false;

        Canvas IDraggable.ScreenCanvas => screenCanvas!;

        Tween? IDraggable.DragTween => _dragTween;

        bool IDraggable.CanDrag => 
            !((this as IHand).IsBusy) && ((this as ITool.IContainer).HeldTool is not null);

        RectTransform? IDraggable.ParentRect => parentRect;

        public float MoveSpeed => moveSpeed;

        void IDraggable.SetDragTween(Tween tween)
        {
            _dragTween = tween;
        }

        Ease IDraggable.DragEase => dragEase;

        void ITool.IContainer.SetHeldTool(ITool? tool)
        {
            HeldTool = tool;
        }
        
    }
}