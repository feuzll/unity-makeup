using System;
using abc.DressUp.Interactions;
using abc.DressUp.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.DressUp.MonoView
{
    [RequireComponent(typeof(Image))]
    public class HandDragArea : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private Hand _hand;
        private Image _image;
        
        public void SetRaycastTarget(bool value) =>
            _image.raycastTarget = value;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("Mouse drag at " + eventData.position);
            new HandDragToScreenPoint(_hand, eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            new HandDragToScreenPoint(_hand, eventData.position);
        }
    }
}