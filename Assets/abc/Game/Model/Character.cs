using System;

namespace abc.Game.Model
{
    public class Character
    {
        private bool _hasAcne = true;
        public  bool  HasAcne => _hasAcne;

        public event Action? SkinChanged;

        public class SkinRole
        {
            private readonly Character _character;
            public SkinRole(Character character, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new ArgumentNullException();
                _character = character;
            }

            public void ClearAcne()
            {
                _character._hasAcne = false;
                _character.SkinChanged?.Invoke();
            }
        }
    }
}