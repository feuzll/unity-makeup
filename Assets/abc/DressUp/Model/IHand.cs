using abc.DressUp.Interactions;
using PrimeTween;
using UnityEngine;

namespace abc.DressUp.Model
{
    public partial interface IHand : ITool.IContainer, IDraggable
    {
        public bool IsBusy { get; protected set; }
        public void Unbusy(Interaction.ExecutionToken token) => IsBusy = false;

        public Vector3 WorldRestPosition { get; }
        
        public float MoveSpeed { get; }
        public new RectTransform Rect { get; }
    }
}