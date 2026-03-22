using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ReturnLipstickContext : ContextBase
    {
        private readonly Hand     _hand;
        private readonly Lipstick _lipstick;

        public ReturnLipstickContext(Hand hand, Lipstick lipstick)
        {
            _hand     = hand;
            _lipstick = lipstick;
        }

        public override void Execute()
        {
            var token = CreateToken();
            new Hand.ToolHolderRole(_hand, token).Drop();
            new Lipstick.StateRole(_lipstick, token)
                .SetState(Lipstick.LipstickState.Shelved);
        }
    }
}