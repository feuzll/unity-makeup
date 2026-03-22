using System;

namespace abc.Game.Model
{
    public partial class Hand
    {
        public class BusyRole
        {
            private readonly Hand _hand;
            public BusyRole(Hand hand, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new  ArgumentNullException();
                _hand = hand;
            }

            public void SetBusy(bool busy)
            {
                _hand._isBusy = busy;
                _hand.HeldToolChanged?.Invoke(); // reuse event — busy affects grabability
            }
        }
    }
}