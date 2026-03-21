using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class DropToolContext : ContextBase
    {
        private readonly Hand _hand;

        public DropToolContext(Hand hand) => _hand = hand;

        public override void Execute()
        {
            var role = new Hand.ToolHolderRole(_hand, CreateToken());
            role.Drop();
        }
    }
}