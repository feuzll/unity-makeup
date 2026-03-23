#nullable enable
using System;
using abc.Game.Contexts;
using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace abc.Game.Unity
{
        [RequireComponent(typeof(RectTransform))]
        public class DragArea : MonoBehaviour
        {
            [SerializeField] private Hand _hand;
            [SerializeField] private ParticlePlayer _particles;
            [SerializeField] private Canvas        _canvas;
            [Tooltip("Pixels of movement before press is treated as drag, not a tap")]
            [SerializeField] private float _tapThreshold = 10f;

            public event System.Action<Vector2>? DragEnded;

            private RectTransform _rectTransform;
            private bool          _isTracking;
            private Vector2       _pressPosition;

            private void Awake() =>
                _rectTransform = GetComponent<RectTransform>();

            private void Update()
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                HandleMouse();
#else
                HandleTouch();
#endif
            }
            
            private void HandleMouse()
            {
                var pos = (Vector2)Input.mousePosition;

                if (Input.GetMouseButtonDown(0) && IsInsideBounds(pos))
                {
                    _pressPosition = pos;
                    _isTracking    = true;
                    FireParticles(pos);
                }

                if (!_isTracking) return;

                if (Input.GetMouseButton(0))
                {
                    TryFireMove(pos);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _isTracking = false;

                    if (Vector2.Distance(pos, _pressPosition) > _tapThreshold)
                        DragEnded?.Invoke(pos);
                }
            }
            
            private void HandleTouch()
            {
                if (Input.touchCount == 0) return;

                // Single finger only — multi-touch ignored for now
                var touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began when IsInsideBounds(touch.position):
                        _pressPosition = touch.position;
                        _isTracking    = true;
                        FireParticles(touch.position);
                        break;

                    case TouchPhase.Moved when _isTracking:
                        TryFireMove(touch.position);
                        break;

                    case TouchPhase.Ended when _isTracking:
                        _isTracking = false;

                        if (Vector2.Distance(touch.position, _pressPosition) > _tapThreshold)
                            DragEnded?.Invoke(touch.position);
                        break;

                    case TouchPhase.Canceled:
                        _isTracking = false;
                        break;
                }
            }
            
            private void FireParticles(Vector2 screenPoint)
            {
                var parentRect = (RectTransform)_hand.transform.parent;
                var cam        = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null : _canvas.worldCamera;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parentRect, screenPoint, cam, out var localPoint))
                    _particles.PlayAt(localPoint);
            }
            
            private void TryFireMove(Vector2 screenPoint)
            {
                if (_hand.Data.HeldTool is null) return;
                if (_hand.Data.IsBusy) return; // expose as public property
                new MoveHandContext(_hand.Data, screenPoint.x, screenPoint.y).Execute();
            }

            private bool IsInsideBounds(Vector2 screenPoint)
            {
                var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : _canvas.worldCamera;

                return RectTransformUtility.RectangleContainsScreenPoint(
                    _rectTransform, screenPoint, cam);
            }
        }
}