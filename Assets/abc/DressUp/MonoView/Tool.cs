using System;
using System.Collections;
using abc.DressUp.Entities;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.DressUp.MonoView
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class Tool : MonoBehaviour, ITool, IPointerClickHandler
    {
        [SerializeField] private ToolSlot initialContainer;
        [SerializeField] private ShakeSettings applySettings;
        [SerializeField] private Canvas screenCanvas;
        [SerializeField] protected ICharacter.ViewState targetCharacterView;
        [SerializeField] private Vector2 handGripOffset;
        
        private ITool.IContainer _container;
        private RectTransform _rectTransform;
        private Vector2 _localContainerPosition;
        [SerializeField] private Vector2 applyPointOffset;

        public void OnPointerClick(PointerEventData eventData)
        {
            _localContainerPosition = _rectTransform.localPosition;
            OnInteract?.Invoke();
        }

        public Canvas ScreenCanvas => screenCanvas;

        public Vector2 HandGripOffset => handGripOffset;

        public Vector2 ApplyPointOffset => applyPointOffset;

        public ITool.IContainer InitialContainer =>  initialContainer;
        
        ITool.IContainer ITool.Container
        {
            get => _container;
            set
            {
                _container = value;
                _rectTransform.SetParent(value.Rect);
            }
        }

        public Vector2 LocalInitialContainerPosition => _localContainerPosition;

        public ShakeSettings ApplySettings => applySettings;
        

        ICharacter.ViewState ITool.TargetCharacterView
        {
            get => targetCharacterView;
        }
        

        public event Action OnInteract;
        void ITool.RaiseOnInteract()
        {
            _localContainerPosition = _rectTransform.localPosition;
            OnInteract?.Invoke();
        }

        protected void Awake()
        {
            targetCharacterView.value = transform.GetSiblingIndex();
            FillReferences();
            if (initialContainer == null) initialContainer = GetComponentInParent<ToolSlot>();
            _container = initialContainer;
            _localContainerPosition = _rectTransform.localPosition;
        }

        protected IEnumerator Start()
        {
            if (TryGetComponent<UnpackFromRectParent>(out var unpacker))
                yield return unpacker.Unpack();
        }

        [ContextMenu(nameof(LogAnchored))]
        public void LogAnchored()
        {
            Debug.Log($"{_rectTransform.anchoredPosition} = anchoredPos");
            Debug.Log($"{_rectTransform.localPosition} = localPos");
        }

        private void OnValidate()
        {
            FillReferences();
        }

        private void FillReferences()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
        }
    }
}