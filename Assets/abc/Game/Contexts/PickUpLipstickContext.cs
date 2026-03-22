using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class PickUpLipstickContext : ContextBase
    {
        private readonly Hand     _hand;
        private readonly Lipstick _lipstick;

        public PickUpLipstickContext(Hand hand, Lipstick lipstick)
        {
            _hand     = hand;
            _lipstick = lipstick;
        }

        public override void Execute()
        {
            var token = CreateToken();
            new Hand.ToolHolderRole(_hand, token).Grab(_lipstick);
            new Lipstick.StateRole(_lipstick, token).SetState(Lipstick.LipstickState.Held);
        }
    }
}