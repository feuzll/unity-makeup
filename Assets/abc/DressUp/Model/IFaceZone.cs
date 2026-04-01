using UnityEngine;

namespace abc.DressUp.Model
{
    public interface IFaceZone
    {
        public Canvas ParentCanvas { get; } 
        public RectTransform Rect { get; }
    }
}