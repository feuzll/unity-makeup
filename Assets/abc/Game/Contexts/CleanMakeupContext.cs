using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class CleanMakeupContext : ContextBase
    {
        private readonly Character _character;

        public CleanMakeupContext(Character character) => _character = character;

        public override void Execute()
        {
            var token = CreateToken();
            new Character.SkinRole(_character, token).RestoreAcne();
            new Character.LipColorRole(_character, token).SetLipColor(-1);
            new Character.EyeColorRole(_character, token).SetEyeColor(-1);
            new Character.BlushColorRole(_character, token).SetBlushColor(-1);
        }
    }
}