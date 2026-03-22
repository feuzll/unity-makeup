using System;
using abc.Game.Contexts;
using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace abc.Game.Unity
{
        public class DragArea : MonoBehaviour, 
            IDragHandler, /*IHand.IDragger,*/ 
            IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
        {
            [SerializeField] private Hand _hand;
            [SerializeField] private Image  _image;
            
            
            public event System.Action? DragEnded;

            private void Awake()
            {
                _hand.Data.HeldToolChanged += () =>
                    _image.enabled = _hand.Data.HeldTool is not null;
                _image.enabled = false; // hand starts empty
            }

            private void Fire(PointerEventData e)
            {
                // Gate: only move hand when it's holding something
                if (_hand.Data.HeldTool is null) return;
                new MoveHandContext(_hand.Data, e.position.x, e.position.y).Execute();
            }

            public void OnDrag(PointerEventData e)        => Fire(e);
            public void OnPointerDown(PointerEventData e) => Fire(e);
            public void OnPointerUp(PointerEventData e)   => DragEnded?.Invoke();
            public void OnPointerClick(PointerEventData e) => Fire(e);
        }
}