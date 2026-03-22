using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class PickUpJarContext : ContextBase
    {
        private readonly Hand _hand;
        private readonly Jar  _jar;

        public PickUpJarContext(Hand hand, Jar jar)
        {
            _hand = hand;
            _jar  = jar;
        }

        public override void Execute()
        {
            var token = CreateToken();
            new Hand.ToolHolderRole(_hand, token).Grab(_jar);
            new Jar.StateRole(_jar, token).SetState(Jar.JarState.Held);
        }
    }
}