using System;

namespace abc.Game.Model
{
    public class Character
    {
        private bool _hasAcne = true;
        private int  _lipColorIndex = -1; // -1 = none applied
        private int _eyeColorIndex   = -1;
        private int _blushColorIndex = -1;
        public  bool  HasAcne => _hasAcne;
        public int  LipColorIndex => _lipColorIndex;
        public int EyeColorIndex   => _eyeColorIndex;
        public int BlushColorIndex => _blushColorIndex;
        public event Action? LipColorChanged;
        public event Action? EyeColorChanged;
        public event Action? BlushColorChanged;
        
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
        
        public class EyeColorRole
        {
            private readonly Character _character;
            public EyeColorRole(Character character, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new ArgumentNullException();
                _character = character;
            }

            public void SetEyeColor(int index)
            {
                _character._eyeColorIndex = index;
                _character.EyeColorChanged?.Invoke();
            }
        }

        public class BlushColorRole
        {
            private readonly Character _character;
            public BlushColorRole(Character character, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new ArgumentNullException();
                _character = character;
            }

            public void SetBlushColor(int index)
            {
                _character._blushColorIndex = index;
                _character.BlushColorChanged?.Invoke();
            }
        }
    }
}