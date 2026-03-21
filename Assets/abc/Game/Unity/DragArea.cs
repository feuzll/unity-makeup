using abc.Game.Contexts;
using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace abc.Game.Unity
{
        public class DragArea : MonoBehaviour, 
            IDragHandler, /*IHand.IDragger,*/ IPointerDownHandler
        {
            [SerializeField] private Hand _hand;

            private void Fire(PointerEventData e) =>
                new MoveHandContext(_hand.Data, e.position.x, e.position.y).Execute();

            public void OnDrag(PointerEventData e)        => Fire(e);
            public void OnPointerDown(PointerEventData e) => Fire(e);
        }
    
}