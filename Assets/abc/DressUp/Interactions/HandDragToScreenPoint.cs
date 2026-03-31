using abc.DressUp.Model;
using abc.Makeup;
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