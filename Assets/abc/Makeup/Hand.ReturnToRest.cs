using PrimeTween;
using UnityEngine;

namespace abc.Makeup
{
    public partial class Hand
    {
        public Tween ReturnToRest()
        {
            return MoveToWorld(WorldRestPosition);
        }
    }
}