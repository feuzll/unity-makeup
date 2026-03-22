using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class SetHandBusyContext : ContextBase
    {
        private readonly Hand _hand;
        private readonly bool     _busy;

        public SetHandBusyContext(Hand hand, bool busy)
        {
            _hand = hand;
            _busy = busy;
        }

        public override void Execute()
        {
            var role = new Hand.BusyRole(_hand, CreateToken());
            role.SetBusy(_busy);
        }
    }
}