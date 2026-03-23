
#nullable enable
using System;
using System.Drawing;

namespace abc.Game.Model
{
    public class Brush : IHand.ITool
    {
        public enum BrushState { Shelved, Held }

        private BrushState _state = BrushState.Shelved;
        private Color      _color = Color.White;

        public BrushState State => _state;
        public Color      Color => _color;

        public event Action? StateChanged;
        public event Action? ColorChanged;

        public class StateRole
        {
            private readonly Brush _brush;
            public StateRole(Brush brush, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new ArgumentNullException(nameof(token));
                _brush = brush;
            }

            public void SetState(BrushState state)
            {
                _brush._state = state;
                _brush.StateChanged?.Invoke();
            }
        }

        public class ColorRole
        {
            private readonly Brush _brush;
            public ColorRole(Brush brush, ContextBase.ExecutionToken token)
            {
                if (token is null) throw new ArgumentNullException(nameof(token));
                _brush = brush;
            }

            public void SetColor(Color color)
            {
                _brush._color = color;
                _brush.ColorChanged?.Invoke();
            }
        }
    }
}