using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace abc.Game.Unity
{
        public class DragArea : MonoBehaviour, IDragHandler, IHand.IUser
        {
            [SerializeField] private Hand target;
            IHand IHand.IUser.Target => target;

            public void OnDrag(PointerEventData eventData)
            {
                Debug.Log(eventData.position);
                (this as IHand.IUser).TryMoveTo(eventData.position.x, eventData.position.y);
            }

        }
    
}