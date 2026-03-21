using System;

namespace abc.Game.Model
{
    public partial class Hand
    {
        public class ToolHolderRole
        {
            private readonly Hand _hand;
            public ToolHolderRole(Hand hand, ContextBase.ExecutionToken token)
            {
                if (token is null)
                    throw new ArgumentNullException();
                _hand = hand;
            }

            public bool CanGrab  => _hand._heldTool is null;
            public bool CanDrop  => _hand._heldTool is not null;

            public void Grab(IHand.ITool tool)
            {
                if (!CanGrab) return;
                _hand._heldTool = tool;
                _hand.HeldToolChanged?.Invoke();
            }

            public IHand.ITool? Drop()
            {
                if (!CanDrop) return null;
                var released    = _hand._heldTool;
                _hand._heldTool = null;
                _hand.HeldToolChanged?.Invoke();
                return released;
            }
        }
        
    }
}