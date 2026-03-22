using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ApplyLipstickContext : ContextBase
    {
        private readonly Character _character;
        private readonly int           _index;

        public ApplyLipstickContext(Character character, int index)
        {
            _character = character;
            _index     = index;
        }

        public override void Execute()
        {
            var role = new Character.LipColorRole(_character, CreateToken());
            role.SetLipColor(_index);
        }
    }
}