using abc.Game.Contexts;
using abc.Game.Model;
using PrimeTween;
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
            if (Data.State != Model.Jar.JarState.Shelved) return;
            if (!_hand.Data.CanGrab) return;

            if (!UISpaceUtil.RectWorldToHandLocal(
                    _rectTransform, _hand, _canvas, out var jarHandLocal)) return;

            var pickupTarget  = jarHandLocal - _gripOffset;
            var readyPosition = Vector2.Lerp(_hand.RestPosition, pickupTarget, 0.5f);

            new SetHandBusyContext(_hand.Data, true).Execute();

            Sequence.Create()
                .Chain(_hand.TweenToAnchored(pickupTarget, _pickupTweenDuration))
                .ChainCallback(() => new PickUpJarContext(_hand.Data, Data).Execute())
                .Chain(_hand.TweenToAnchored(readyPosition, _pickupTweenDuration))
                .ChainCallback(() => new SetHandBusyContext(_hand.Data, false).Execute());
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
            new SetHandBusyContext(_hand.Data, true).Execute();

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceLocalPos)) return;

            var jarCenterWorld = _rectTransform.TransformPoint(_rectTransform.rect.center);

            if (!UISpaceUtil.WorldToHandLocal(
                    _shelfWorldPosition, _hand, _canvas,
                    out var shelfHandLocal)) return;

            Sequence.Create()
                .Chain(_hand.TweenToAnchored(faceLocalPos, _applyTweenDuration))
                .Chain(Tween.ShakeLocalPosition(_handRect,
                    new Vector3(_shakeStrength, _shakeStrength, 0f), _shakeDuration))
                .ChainCallback(() => new ClearAcneContext(_character.Data).Execute())
                .Chain(_hand.TweenToAnchored(shelfHandLocal - _gripOffset, _applyTweenDuration))
                .ChainCallback(() => new ReturnJarContext(_hand.Data, Data).Execute())
                .Chain(_hand.TweenToRest())
                .ChainCallback(() => new SetHandBusyContext(_hand.Data, false).Execute());
        }
    }
}