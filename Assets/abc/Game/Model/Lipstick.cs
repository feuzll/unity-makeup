#nullable enable
using System;

namespace abc.Game.Model
{
    public class Lipstick : IHand.ITool
    {
        public enum LipstickState { Shelved, Held }

        private LipstickState _state = LipstickState.Shelved;
        public  LipstickState  State => _state;

        public event Action? StateChanged;

        public class StateRole
        {
            private readonly Lipstick _lipstick;

            public StateRole(Lipstick lipstick, ContextBase.ExecutionToken token)
            {
                if (token is null)
                    throw new ArgumentNullException();
                _lipstick = lipstick;
            }

            public void SetState(LipstickState state)
            {
                _lipstick._state = state;
                _lipstick.StateChanged?.Invoke();
            }
        }
    }
}