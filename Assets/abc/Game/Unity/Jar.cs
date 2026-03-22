using abc.Game.Contexts;
using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public class Jar : MonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Hand       _hand;
        [SerializeField] private DragArea   _dragArea;
        [SerializeField] private Character  _character;
        [SerializeField] private RectTransform       _faceZone;

        [Header("Timing")]
        [SerializeField] private float _pickupTweenDuration = 0.35f;
        [SerializeField] private float _applyTweenDuration  = 0.3f;
        [SerializeField] private float _shakeDuration       = 0.4f;
        [SerializeField] private float _shakeStrength       = 18f;
        
        public Game.Model.Jar Data { get; } = new();

        private RectTransform _rectTransform;
        private Vector2       _shelfAnchoredPosition; // recorded on Start, never changes

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Data.StateChanged += OnStateChanged;
        }

        private void Start()
        {
            _shelfAnchoredPosition = _rectTransform.anchoredPosition;
            _dragArea.DragEnded   += OnDragEnded;
        }

        private void OnDestroy()
        {
            Data.StateChanged     -= OnStateChanged;
            _dragArea.DragEnded   -= OnDragEnded;
        }
        
        public void OnPointerClick(PointerEventData _)
        {
            if (Data.State != Game.Model.Jar.JarState.Shelved) return;
            if (_hand.Data.HeldTool is not null) return;

            var readyPosition = Vector2.Lerp(
                _hand.RestPosition,
                _shelfAnchoredPosition,
                0.5f);

            // 1. Hand tweens to jar
            _hand.TweenToAnchored(_shelfAnchoredPosition, _pickupTweenDuration)
                .OnComplete(() =>
                {
                    // 2. State: jar is now held
                    new PickUpJarContext(_hand.Data, Data).Execute();

                    // 3. Hand (with jar) moves to ready position
                    _hand.TweenToAnchored(readyPosition, _pickupTweenDuration);
                });
        }

        private void OnStateChanged()
        {
            // Parent jar to hand while held, return to shelf parent when dropped
            if (Data.State == Game.Model.Jar.JarState.Held)
            {
                _rectTransform.SetParent(_hand.transform, worldPositionStays: true);
                _rectTransform.anchoredPosition = new Vector2(0, -20f); // grip offset
            }
            else
            {
                // Re-parent to original shelf parent (store reference if needed)
                _rectTransform.SetParent(transform.parent, worldPositionStays: true);
            }
        }

        // ── Application ───────────────────────────────────────────────────────
        
        private void OnDragEnded()
        {
            if (Data.State != Game.Model.Jar.JarState.Held) return;
            if (!IsHandOverFace()) return;

            // Disable drag during the scripted sequence
            _dragArea.enabled = false;

            var faceCenter = _faceZone.anchoredPosition;

            // 1. Hand tweens to face center
            _hand.TweenToAnchored(faceCenter, _applyTweenDuration)
                .OnComplete(() =>
                {
                    // 2. Shake
                    PrimeTween.Tween.ShakeLocalPosition(
                            _hand.GetComponent<RectTransform>(),
                            strength: new Vector3(_shakeStrength, _shakeStrength), _shakeDuration)
                        .OnComplete(() =>
                        {
                            // 3. Apply — one atomic context fires here
                            new ApplyFaceCreamContext(
                                _hand.Data, Data, _character.Data).Execute();

                            // 4. Hand carries jar back to shelf position,
                            //    then jar detaches and hand returns to rest
                            _hand.TweenToAnchored(_shelfAnchoredPosition, _applyTweenDuration)
                                .OnComplete(() =>
                                {
                                    // jar is already reparented by OnStateChanged
                                    _rectTransform.anchoredPosition = _shelfAnchoredPosition;

                                    _hand.TweenToRest()
                                        .OnComplete(() => _dragArea.enabled = true);
                                });
                        });
                });
        }

        private bool IsHandOverFace()
        {
            var handWorldPos = ((RectTransform)_hand.transform).position;
            var screenPos    = RectTransformUtility.WorldToScreenPoint(
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                handWorldPos);

            return RectTransformUtility.RectangleContainsScreenPoint(
                _faceZone, screenPos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera);
        }
    }
}