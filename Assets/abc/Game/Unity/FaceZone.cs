#nullable enable
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class FaceZone : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Hand _hand;
        [SerializeField] private Canvas        _canvas;
        
        private RectTransform _rectTransform;
        
        private void Awake() =>
            _rectTransform = GetComponent<RectTransform>();
        
        private void OnValidate()
        {
            GetComponent<Image>().raycastTarget = true;
        }

        public event Action? ClickedOnFace;

        public bool IsHandOver
        {
            get
            {
                var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null : _canvas.worldCamera;

                var handWorldPos = ((RectTransform)_hand.transform).position;
                var screenPos    = RectTransformUtility.WorldToScreenPoint(cam, handWorldPos);

                return RectTransformUtility.RectangleContainsScreenPoint(
                    _rectTransform, screenPos, cam);
            }
        }
        
        public bool ContainsScreenPoint(Vector2 screenPoint)
        {
            var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : _canvas.worldCamera;
            return RectTransformUtility.RectangleContainsScreenPoint(
                _rectTransform, screenPoint, cam);
        }
        
        public void OnPointerClick(PointerEventData _) => ClickedOnFace?.Invoke();
    }
}