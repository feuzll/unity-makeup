using System;

namespace abc.Game.Model
{
    public partial class Hand
    {
        public class MoveableRole
        {
            private readonly Hand _hand;
            public MoveableRole(Hand hand, ContextBase.ExecutionToken token)
            {
                if (token is null)
                    throw new ArgumentNullException();
                _hand = hand;
            }

            public void MoveTo(float x, float y)
            {
                _hand._position = (x, y);
                _hand.PositionChanged?.Invoke();
            }
        }
    }
}