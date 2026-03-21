using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.Game.Unity
{
    public partial class Hand
    {
        public class DragArea : MonoBehaviour, IDragHandler, IHand.IUser
        {
            [SerializeField] private Hand _target;
            IHand IHand.IUser.Target => _target;

            public void OnDrag(PointerEventData eventData)
            {
                (this as IHand.IUser).TryMoveTo(eventData.position.x, eventData.position.y);
            }

        }
    }
    
}