using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.Game.Unity
{
    public partial class Hand : MonoBehaviour, IHand
    {
        void IHand.RequestMoveTo(float x, float y)
        {
            
        }
    }
}