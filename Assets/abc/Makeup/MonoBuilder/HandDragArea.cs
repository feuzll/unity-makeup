#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.Makeup.MonoBuilder
{
    public class HandDragArea : MonoBehaviour,
        IDragHandler, IPointerDownHandler,
        //IBeginDragHandler, IEndDragHandler,
        Makeup.Hand.DragArea.IInput
    {
        [SerializeField] Hand handBuilder;
        private Makeup.Hand.DragArea? _model;
        private Makeup.Hand _hand;

        private Makeup.Hand.DragArea Model
        {
            get
            {
                _model ??= new Makeup.Hand.DragArea(handBuilder.Model);
                return _model;
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("Mouse drag at " + eventData.position);
            (this as Makeup.Hand.DragArea.IInput).RaiseDragEvent(Model, eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            (this as Makeup.Hand.DragArea.IInput).RaiseDragEvent(Model, eventData.position);
        }
        //public void OnBeginDrag(PointerEventData eventData) { }
        //public void OnEndDrag(PointerEventData eventData) { }
    }
}