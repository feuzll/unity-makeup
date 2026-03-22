using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ReturnJarContext : ContextBase
    {
        private readonly Hand _hand;
        private readonly Jar  _jar;

        public ReturnJarContext(Hand hand, Jar jar)
        {
            _hand = hand;
            _jar  = jar;
        }

        public override void Execute()
        {
            var token = CreateToken();
            new Hand.ToolHolderRole(_hand, token).Drop();
            new Jar.StateRole(_jar, token).SetState(Jar.JarState.Shelved);
        }
    }
}