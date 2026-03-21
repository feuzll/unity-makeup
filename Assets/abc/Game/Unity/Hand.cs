using System;
using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public partial class Hand : MonoBehaviour, IHand
    {
        [SerializeField] private Canvas canvas;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void IHand.RequestMoveTo(float x, float y)
        {
            var parentRect = (RectTransform)_rectTransform.parent;
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect, new Vector2(x, y), cam, out var localPoint))
            {
                _rectTransform.anchoredPosition = localPoint;
            }
        }
    }
}