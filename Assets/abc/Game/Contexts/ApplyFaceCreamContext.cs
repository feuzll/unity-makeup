using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ApplyFaceCreamContext : ContextBase
    {
        private readonly Hand      _hand;
        private readonly Jar       _jar;
        private readonly Character _character;

        public ApplyFaceCreamContext(Hand hand, Jar jar, Character character)
        {
            _hand      = hand;
            _jar       = jar;
            _character = character;
        }

        public override void Execute()
        {
            var token = CreateToken();
            new Character.SkinRole(_character, token).ClearAcne();
            new Hand.ToolHolderRole(_hand, token).Drop();
            new Jar.StateRole(_jar, token).SetState(Jar.JarState.Shelved);
        }
    }
}