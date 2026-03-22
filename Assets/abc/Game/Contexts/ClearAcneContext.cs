using abc.Game.Model;

namespace abc.Game.Contexts
{
    public class ClearAcneContext : ContextBase
    {
        private readonly Character _character;

        public ClearAcneContext(Character character) => _character = character;

        public override void Execute()
        {
            var role = new Character.SkinRole(_character, CreateToken());
            role.ClearAcne();
        }
    }
}