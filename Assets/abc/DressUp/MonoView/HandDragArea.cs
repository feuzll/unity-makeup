using abc.DressUp.Interactions;
using abc.DressUp.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.DressUp.View
{
    public class HandDragArea : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private IDraggable _hand;
        
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