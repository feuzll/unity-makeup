#nullable enable
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.Makeup.MonoBuilder
{
    [RequireComponent(typeof(RectTransform))]
    public class FaceZone : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerDownHandler
    {
        public event Action? OnInteract;
        private RectTransform? _rectTransform;

        public RectTransform Rect
        {
            get => _rectTransform ??= GetComponent<RectTransform>();
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