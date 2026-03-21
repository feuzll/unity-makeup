using System;

namespace abc.Game.Model
{
    public partial class Hand
    {
        private (float x, float y) _position;
        private IHand.ITool?       _heldTool;

        public (float x, float y) Position  => _position;
        public IHand.ITool?        HeldTool  => _heldTool;

        public event Action? PositionChanged;
        public event Action? HeldToolChanged;
    }
}