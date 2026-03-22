using System;

namespace abc.Game.Model
{
    public partial class Hand
    {
        private (float x, float y) _position;
        private IHand.ITool?       _heldTool;
        private bool _isBusy = false;
        
        public (float x, float y) Position  => _position;
        public IHand.ITool?        HeldTool  => _heldTool;
        public bool CanGrab => _heldTool is null && !_isBusy;
        public bool IsBusy  => _isBusy;

        public event Action? PositionChanged;
        public event Action? HeldToolChanged;
    }
}