using abc.DressUp.Entities;
using UnityEngine;

namespace abc.DressUp.Interactions
{
    public class HandDragToScreenPoint : Interaction
    {
        public HandDragToScreenPoint(IDraggable hand, Vector2 screenPos) : base()
        {
            hand.MaybeDragToScreenXY(Token, screenPos);
        }
        
    }
}