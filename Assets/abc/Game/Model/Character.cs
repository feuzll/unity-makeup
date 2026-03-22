using System;

namespace abc.Game.Model
{
    public class Character
    {
        private bool _hasAcne = true;
        private int  _lipColorIndex = -1; // -1 = none applied
        public  bool  HasAcne => _hasAcne;
        public int  LipColorIndex => _lipColorIndex;
        public event Action? LipColorChanged;
        
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
        
        public class LipColorRole
        {
            private readonly Character _character;
            public LipColorRole(Character character, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new ArgumentNullException();
                _character = character;
            }

            public void SetLipColor(int index)
            {
                _character._lipColorIndex = index;
                _character.LipColorChanged?.Invoke();
            }
        }
    }
}