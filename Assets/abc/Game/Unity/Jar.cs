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
        [SerializeField] private DragArea _dragArea;
        [SerializeField] private Character  _character;
        [SerializeField] private FaceZone   _faceZone;
        
        
        [Header("Timing")]
        [SerializeField] private float _pickupTweenDuration = 0.35f;
        [SerializeField] private float _applyTweenDuration  = 0.3f;
        [SerializeField] private float _returnCreamDuration = 0.3f;
        [SerializeField] private float _shakeDuration       = 0.4f;
        [SerializeField] private float _shakeStrength       = 18f;
        
        [Header("Grabbing")]
        [SerializeField] private Vector2 _gripOffset = new Vector2(0, -20f);
        
        public Game.Model.Jar Data { get; } = new();

        private RectTransform _rectTransform;
        private Image Image => GetComponent<Image>();
        private Vector2 _shelfAnchoredPosition; // recorded on Start, never changes
        private Vector3 _shelfWorldPosition;
        private Transform _shelfParent;
        private RectTransform _handRect;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Data.StateChanged += OnStateChanged;
            _handRect = _hand.GetComponent<RectTransform>();
        }

        private void Start()
        {
            _shelfParent           = transform.parent; // store before any reparenting
            _shelfAnchoredPosition = _rectTransform.anchoredPosition;
            _shelfWorldPosition     = _rectTransform.position; // world space, no conversion needed
            _dragArea.DragEnded      += OnDragEnded;
            _faceZone.ClickedOnFace  += OnFaceClicked;
        }
        
        private void OnDragEnded(Vector2 screenPos)
        {
            if (Data.State != Model.Jar.JarState.Held) return;
            if (!_faceZone.ContainsScreenPoint(screenPos)) return;
            StartApplySequence();
        }

        private void OnFaceClicked()
        {
            if (Data.State != Model.Jar.JarState.Held) return;
            StartApplySequence();
        }

        private void OnDestroy()
        {
            Data.StateChanged     -= OnStateChanged;
            _faceZone.ClickedOnFace += OnFaceClicked;
            _dragArea.DragEnded      -= OnDragEnded;
        }
        
        public void OnPointerClick(PointerEventData _)
        {
            if (Data.State != Game.Model.Jar.JarState.Shelved) return;
            if (!_hand.Data.CanGrab) return;
            
            new SetHandBusyContext(_hand.Data, true).Execute(); // ← busy from first tween
            
            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)transform, _hand, _canvas, out var jarHandLocal)) return;

            // compensate so jar lands at grip offset naturally after reparent
            var pickupTarget  = jarHandLocal - _gripOffset;
            var readyPosition = Vector2.Lerp(_hand.RestPosition, pickupTarget, 0.5f);
            
            
            // 1. Hand tweens to jar
            _hand.TweenToAnchored(pickupTarget, _pickupTweenDuration)
                .OnComplete(() =>
                {
                    // 2. State: jar is now held
                    new PickUpJarContext(_hand.Data, Data).Execute();

                    // 3. Hand (with jar) moves to ready position
                    _hand.TweenToAnchored(readyPosition, _pickupTweenDuration)
                        .OnComplete(() =>
                            new SetHandBusyContext(_hand.Data, false).Execute()); // ← free after ready
                });
        }

        private void OnStateChanged()
        {
            // Parent jar to hand while held, return to shelf parent when dropped
            if (Data.State == Game.Model.Jar.JarState.Held)
            {
                _rectTransform.SetParent(_hand.transform, worldPositionStays: true);
                Image.raycastTarget = false;
            }
            else
            {
                _rectTransform.SetParent(_shelfParent, worldPositionStays: true);
                Image.raycastTarget = true;
            }
        }

        // ── Application ───────────────────────────────────────────────────────
        
        private void StartApplySequence()
        {
            /*if (Data.State != Game.Model.Jar.JarState.Held) return;
            if (!_faceZone.IsHandOver) return;*/

            new SetHandBusyContext(_hand.Data, true).Execute();
            
            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas, out var faceLocalPos)) return;

            // 1. Hand tweens to face center
            _hand.TweenToAnchored(faceLocalPos, _applyTweenDuration)
                .OnComplete(() =>
                {
                    // 2. Shake
                    PrimeTween.Tween.ShakeLocalPosition(
                            _handRect,
                            strength: new Vector3(_shakeStrength, _shakeStrength), _shakeDuration)
                        .OnComplete(() =>
                        {
                            // acne clears exactly here — after shake, before moving away
                            new ClearAcneContext(_character.Data).Execute();
                            
                            // shelf world pos is always correct regardless of jar's current parent
                            //var shelfWorldPos = _shelfParent.TransformPoint(_shelfAnchoredPosition);

                            if (!UISpaceUtil.WorldToHandLocal(
                                    _shelfWorldPosition, _hand, _canvas, out var shelfHandLocal)) return;
                            
                            // compensate grip offset so jar lands exactly on shelf after reparent
                            var returnTarget = shelfHandLocal - _gripOffset;

                            // 4. Hand carries jar back to shelf position,
                            //    then jar detaches and hand returns to rest
                            _hand.TweenToAnchored(returnTarget, _returnCreamDuration)
                                .OnComplete(() =>
                                {
                                    // jar drops here — after hand arrives at shelf
                                    new ReturnJarContext(_hand.Data, Data).Execute();

                                    _hand.TweenToRest().OnComplete(() =>
                                        new SetHandBusyContext(_hand.Data, false).Execute());
                                });
                        });
                });
        }
    }
}