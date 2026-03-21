using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class GrabToolContext : ContextBase
    {
        private readonly Hand    _hand;
        private readonly IHand.ITool _tool;

        public GrabToolContext(Hand hand, IHand.ITool tool)
        {
            _hand = hand;
            _tool = tool;
        }
        
        public override void Execute()
        {
            var role = new Hand.ToolHolderRole(_hand, CreateToken());
            role.Grab(_tool);
        }
    }
}