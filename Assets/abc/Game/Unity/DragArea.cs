using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace abc.Game.Unity
{
        public class DragArea : MonoBehaviour, 
            IDragHandler, IHand.IDragger, IPointerDownHandler
        {
            [SerializeField] private Hand target;
            IHand IHand.IDragger.Target => target;

            public void OnDrag(PointerEventData eventData)
            {
                Debug.Log(eventData.position);
                (this as IHand.IDragger).TryDragTo(eventData.position.x, eventData.position.y);
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                (this as IHand.IDragger).TryDragTo(eventData.position.x, eventData.position.y);
            }
        }
    
}