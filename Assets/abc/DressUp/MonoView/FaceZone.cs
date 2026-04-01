#nullable enable
using System;
using abc.DressUp.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.DressUp.MonoView
{
    [RequireComponent(typeof(RectTransform))]
    public class FaceZone : MonoBehaviour, IFaceZone,
        IPointerEnterHandler,
        IPointerDownHandler
    {
        [SerializeField] private Canvas parentCanvas;
        private RectTransform _rect;

        public event Action? OnInteract;
        
        public Canvas ParentCanvas => parentCanvas;

        public RectTransform Rect => _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pressure > 0)
            {
                Debug.Log("OnPointerDown on face");
                OnInteract?.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnInteract?.Invoke();
            Debug.Log("OnPointerDown on face");
        }
    }
}