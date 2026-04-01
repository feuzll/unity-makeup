using abc.DressUp.Interactions;
using abc.DressUp.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.DressUp.MonoView
{
    public class HandDragArea : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private Hand _hand;
        
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