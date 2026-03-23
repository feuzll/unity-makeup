
#nullable enable


using System;

namespace abc.Game.Model
{
    public readonly struct BrushColor
    {
        public readonly float R, G, B, A;
        public BrushColor(float r, float g, float b, float a)
        {
            R = r; G = g; B = b; A = a;
        }
    }
    
    public class Brush : IHand.ITool
    {
        public enum BrushState { Shelved, Held }

        private BrushState _state = BrushState.Shelved;

        public BrushState State => _state;
        private BrushColor _color = new BrushColor(1f, 1f, 1f, 1f);
        public  BrushColor  Color => _color;

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

            public void SetColor(BrushColor color)
            {
                _brush._color = color;
                _brush.ColorChanged?.Invoke();
            }
        }
    }
}