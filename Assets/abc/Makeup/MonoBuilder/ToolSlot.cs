using System;
using UnityEngine;

namespace abc.Makeup.MonoBuilder
{
    [RequireComponent(typeof(RectTransform))]
    public class ToolSlot : MonoBehaviour, Makeup.Tool.IContainer
    {
        private RectTransform _rect;
        private Makeup.Tool _heldTool;
        
        public RectTransform Rect => _rect ??= GetComponent<RectTransform>();

        Makeup.Tool Makeup.Tool.IContainer.HeldTool
        {
            get => _heldTool;
            set => _heldTool = value;
        }
    }
}