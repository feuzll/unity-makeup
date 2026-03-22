using abc.Game.Contexts;
using abc.Game.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class Jar : MonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Hand       _hand;
        [SerializeField] private DragArea   _dragArea;
        [SerializeField] private Character  _character;
        [SerializeField] private FaceZone   _faceZone;
        
        
        [Header("Timing")]
        [SerializeField] private float _pickupTweenDuration = 0.35f;
        [SerializeField] private float _applyTweenDuration  = 0.3f;
        [SerializeField] private float _shakeDuration       = 0.4f;
        [SerializeField] private float _shakeStrength       = 18f;
        
        public Game.Model.Jar Data { get; } = new();

        private RectTransform _rectTransform;
        private Image Image => GetComponent<Image>();
        private Vector2       _shelfAnchoredPosition; // recorded on Start, never changes

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Data.StateChanged += OnStateChanged;
        }

        private void Start()
        {
            _shelfAnchoredPosition = _rectTransform.anchoredPosition;
            _dragArea.DragEnded   += TryApplyCream;
            _faceZone.ClickedOnFace += TryApplyCream;
        }

        private void OnDestroy()
        {
            Data.StateChanged     -= OnStateChanged;
            _dragArea.DragEnded -= TryApplyCream;
            _faceZone.ClickedOnFace += TryApplyCream;
        }
        
        public void OnPointerClick(PointerEventData _)
        {
            if (Data.State != Game.Model.Jar.JarState.Shelved) return;
            if (_hand.Data.HeldTool is not null) return;
            
            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)transform, _hand, _canvas, out var jarLocalPos)) return;

            var readyPosition = Vector2.Lerp(_hand.RestPosition, jarLocalPos, 0.5f);

            // 1. Hand tweens to jar
            _hand.TweenToAnchored(jarLocalPos, _pickupTweenDuration)
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
                Image.raycastTarget = false;
            }
            else
            {
                // Re-parent to original shelf parent (store reference if needed)
                _rectTransform.SetParent(transform.parent, worldPositionStays: true);
                Image.raycastTarget = true;
            }
        }

        // ── Application ───────────────────────────────────────────────────────
        
        private void TryApplyCream()
        {
            if (Data.State != Game.Model.Jar.JarState.Held) return;
            if (!_faceZone.IsHandOver) return;
            
            // Disable drag during the scripted sequence
            _dragArea.enabled = false;

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas, out var faceLocalPos)) return;

            // 1. Hand tweens to face center
            _hand.TweenToAnchored(faceLocalPos, _applyTweenDuration)
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

                            if (!UISpaceUtil.RectWorldToHandLocal(
                                    _rectTransform, _hand, _canvas, out var jarReturnPos)) return;
                            // 4. Hand carries jar back to shelf position,
                            //    then jar detaches and hand returns to rest
                            _hand.TweenToAnchored(jarReturnPos, _applyTweenDuration)
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
    }
}