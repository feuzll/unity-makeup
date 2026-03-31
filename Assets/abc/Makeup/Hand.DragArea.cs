#nullable enable
using System;
using UnityEngine;

namespace abc.Makeup
{
    public partial class Hand
    {
        public partial class DragArea
        {
            private readonly Hand _hand;

            public DragArea(Hand hand)
            {
                _hand = hand;
                OnDrag += (point) => _hand.TryDragToScreenPoint(point);
            }

            public event Action<Vector2>? OnDrag;
        
            public interface IInput
            {
                public void RaiseDragEvent(DragArea model, Vector2 position) => model.OnDrag?.Invoke(position);
            }
        }
    }
}