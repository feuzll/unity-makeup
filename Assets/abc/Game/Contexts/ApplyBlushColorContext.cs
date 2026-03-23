using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ApplyBlushColorContext : ContextBase
    {
        private readonly Character _character;
        private readonly int           _index;

        public ApplyBlushColorContext(Character character, int index)
        {
            _character = character;
            _index     = index;
        }

        public override void Execute()
        {
            var role = new Character.BlushColorRole(_character, CreateToken());
            role.SetBlushColor(_index);
        }
    }
}