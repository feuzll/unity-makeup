using System;
using abc.DressUp.Model;
using UnityEngine;

namespace abc.DressUp.View
{
    [RequireComponent(typeof(RectTransform))]
    public class ToolSlot : MonoBehaviour, ITool.IContainer
    {
        private RectTransform _rect;
        private ITool _heldTool;
        private Vector2 _toolBindPosition;

        public RectTransform Rect => _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _heldTool = GetComponentInChildren<ITool>();
            _toolBindPosition = _heldTool.LocalContainerPosition;
        }

        ITool ITool.IContainer.HeldTool => _heldTool;

        public Vector2 ToolLocalBindPosition => _toolBindPosition;

        void ITool.IContainer.SetHeldTool(ITool tool)
        {
            _heldTool = tool;
        }
    }
}