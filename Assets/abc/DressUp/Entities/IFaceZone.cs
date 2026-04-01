using UnityEngine;

namespace abc.DressUp.Entities
{
    public interface IFaceZone
    {
        public Canvas ParentCanvas { get; } 
        public RectTransform Rect { get; }
    }
}