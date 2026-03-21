using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class MoveHandContext : ContextBase
    {
        private readonly Hand _hand;
        private readonly float    _x, _y;

        public MoveHandContext(Hand hand, float x, float y)
        {
            _hand = hand;
            _x    = x;
            _y    = y;
        }
        
        public override void Execute()
        {
            var role = new Hand.MoveableRole(_hand, CreateToken());
            role.MoveTo(_x, _y);
        }
    }
}