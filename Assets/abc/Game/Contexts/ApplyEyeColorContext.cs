using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ApplyEyeColorContext : ContextBase
    {
        private readonly Character _character;
        private readonly int           _index;

        public ApplyEyeColorContext(Character character, int index)
        {
            _character = character;
            _index     = index;
        }

        public override void Execute()
        {
            var role = new Character.EyeColorRole(_character, CreateToken());
            role.SetEyeColor(_index);
        }
    }
}