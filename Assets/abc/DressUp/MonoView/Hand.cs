#nullable enable
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
        private float _dragSpeed;

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
            _worldRestPosition = parentRect!.TransformPoint(
                rectTransform!.anchoredPosition);
        }

        private void OnValidate()
        {
            TryFillReferences();
        }

        RectTransform IHand.Rect => rectTransform!;

        float IDraggable.DragSpeed => _dragSpeed;

        RectTransform IDraggable.Rect => rectTransform!;
        RectTransform ITool.IContainer.Rect => rectTransform!;

        public ITool? HeldTool { get; private set; }

        public Vector2 ToolLocalBindPosition => toolLocalBindPosition;

        bool IHand.IsBusy { get; set; } = false;

        Canvas IDraggable.ScreenCanvas => screenCanvas!;

        Tween? IDraggable.DragTween { get; } = null;

        bool IDraggable.CanDrag => 
            (!(this as IHand).IsBusy) && ((this as ITool.IContainer).HeldTool is not null);

        RectTransform? IDraggable.ParentRect => parentRect;

        public float MoveSpeed => moveSpeed;

        void IDraggable.SetDragTween(Tween tween)
        {
            throw new System.NotImplementedException();
        }

        Ease IDraggable.DragEase => dragEase;

        void ITool.IContainer.SetHeldTool(ITool? tool)
        {
            HeldTool = tool;
        }
        
    }
}