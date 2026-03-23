#nullable enable
using abc.Game.Contexts;
using PrimeTween;
using UnityEngine;

namespace abc.Game.Unity
{
    public class LipsticksPage : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Hand     _hand;
        [SerializeField] private DragArea  _dragArea;
        [SerializeField] private Character _character;
        [SerializeField] private FaceZone  _faceZone;
        [SerializeField] private Canvas             _canvas;
        [SerializeField] private Lipstick[] _lipsticks;

        [Header("Timing")]
        [SerializeField] private float _pickupTweenDuration = 0.35f;
        [SerializeField] private float _applyTweenDuration  = 0.3f;
        [SerializeField] private float _returnDuration      = 0.3f;
        [SerializeField] private float _shakeDuration       = 0.4f;
        [SerializeField] private float _shakeStrength       = 18f;

        [Header("Grip")]
        [SerializeField] private Vector2 _gripOffset = new Vector2(0, -20f);

        [Header("Lips")]
        [SerializeField] private Vector2 _lipsOffset = new Vector2(0, -30f);
        
        // currently held lipstick — null when hand is empty
        private Lipstick? _held;
        private RectTransform _handRect;

        private void Start()
        {
            _handRect = _hand.GetComponent<RectTransform>();
            
            foreach (var l in _lipsticks)
                l.Clicked += OnLipstickClicked;

            _dragArea.DragEnded += OnDragEnded;
            _faceZone.ClickedOnFace    += OnFaceClicked;
        }

        private void OnDestroy()
        {
            foreach (var l in _lipsticks)
                l.Clicked -= OnLipstickClicked;

            _dragArea.DragEnded -= OnDragEnded;
            _faceZone.ClickedOnFace    -= OnFaceClicked;
        }

        private void OnFaceClicked()
        {
            if (_held is null) return;
            StartApplySequence(_held);
        }
        
        private void OnDragEnded(Vector2 screenPos)
        {
            if (_held is null) return;
            if (!_faceZone.ContainsScreenPoint(screenPos)) return;
            StartApplySequence(_held);
        }
        
        // ── Pickup ────────────────────────────────────────────────────────────

        private void OnLipstickClicked(Lipstick lipstick)
        {
            if (!_hand.Data.CanGrab) return;

            if (!UISpaceUtil.RectWorldToHandLocal(
                    lipstick.Rect, _hand, _canvas, out var lipstickHandLocal)) return;

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceHandLocal)) return;

            var pickupTarget  = lipstickHandLocal - _gripOffset;
            var readyPosition = Vector2.Lerp(pickupTarget, faceHandLocal, 0.5f);

            new SetHandBusyContext(_hand.Data, true).Execute();

            Sequence.Create()
                .Chain(_hand.TweenToAnchored(pickupTarget))
                .ChainCallback(() => GrabLipstick(lipstick))
                .Chain(_hand.TweenToAnchored(pickupTarget, readyPosition))
                .ChainCallback(() => new SetHandBusyContext(_hand.Data, false).Execute());
        }

        private void GrabLipstick(Lipstick lipstick)
        {
            _held = lipstick;
            lipstick.SetHandCanvas(_hand.GetComponent<Canvas>());
            lipstick.Rect.SetParent(_hand.transform, worldPositionStays: true);
            new PickUpLipstickContext(_hand.Data, lipstick.Data).Execute();
        }
        
        // ── Application ───────────────────────────────────────────────────────

        private void StartApplySequence(Lipstick lipstick)
        {
            new SetHandBusyContext(_hand.Data, true).Execute();

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceLocalPos)) return;

            if (!UISpaceUtil.WorldToHandLocal(
                    lipstick.ShelfWorldPosition, _hand, _canvas,
                    out var shelfHandLocal)) return;

            var faceTarget   = faceLocalPos + _lipsOffset;
            var returnTarget = shelfHandLocal - _gripOffset;

            Sequence.Create()
                .Chain(_hand.TweenToAnchored(faceTarget))
                .Chain(Tween.ShakeLocalPosition(_handRect,
                    new Vector3(_shakeStrength, 0f, 0f), _shakeDuration))
                .ChainCallback(() => new ApplyLipstickContext(_character.Data, lipstick.Index).Execute())
                .Chain(_hand.TweenToAnchored(faceTarget, returnTarget))
                .ChainCallback(() => ReturnLipstick(lipstick))
                .Chain(_hand.TweenToRest())
                .ChainCallback(() => new SetHandBusyContext(_hand.Data, false).Execute());
        }
        private void RaiseApplyLipstickContext(Lipstick lipstick)
        {
            new ApplyLipstickContext(_character.Data, lipstick.Index).Execute();
        }
        private void ReturnLipstick(Lipstick lipstick)
        {
            new ReturnLipstickContext(_hand.Data, lipstick.Data).Execute();
            lipstick.Rect.SetParent(lipstick.ShelfParent, worldPositionStays: true);
            _held = null;
        }
    }
}