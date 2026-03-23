using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ReturnBrushContext : ContextBase
    {
        private readonly Hand  _hand;
        private readonly Brush _brush;

        public ReturnBrushContext(Hand hand, Brush brush)
        {
            _hand  = hand;
            _brush = brush;
        }

        public override void Execute()
        {
            var token = CreateToken();
            new Hand.ToolHolderRole(_hand, token).Drop();
            new Brush.StateRole(_brush, token).SetState(Brush.BrushState.Shelved);
        }
        
    }
}