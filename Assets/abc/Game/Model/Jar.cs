#nullable enable
using System;

namespace abc.Game.Model
{
    public class Jar : IHand.ITool
    {
        public enum JarState { Shelved, Held }

        private JarState _state = JarState.Shelved;
        public  JarState  State => _state;
        
        public event Action? StateChanged;

        public class StateRole
        {
            private readonly Jar _jar;

            public StateRole(Jar jar, ContextBase.ExecutionToken token)
            {
                if (token is null)
                    throw new ArgumentNullException();
                _jar = jar;
            }

            public void SetState(JarState state)
            {
                _jar._state = state;
                _jar.StateChanged?.Invoke();
            }
        }
    }
}