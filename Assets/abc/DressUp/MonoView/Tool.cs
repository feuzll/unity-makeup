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
        
        private ITool.IContainer _container;
        private RectTransform _rectTransform;
        private Vector2 _localContainerPosition;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnInteract?.Invoke();
        }

        public Canvas ScreenCanvas => screenCanvas;

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
            OnInteract?.Invoke();
        }

        protected void Awake()
        {
            FillReferences();
            if (initialContainer == null) initialContainer = GetComponentInParent<ToolSlot>();
            _container = initialContainer;
            _localContainerPosition = _rectTransform.localPosition;
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