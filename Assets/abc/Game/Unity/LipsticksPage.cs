#nullable enable
using abc.Game.Contexts;
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

        [Header("Grip")]
        [SerializeField] private Vector2 _gripOffset = new Vector2(0, -20f);

        // currently held lipstick — null when hand is empty
        private Lipstick? _held;

        private void Start()
        {
            foreach (var l in _lipsticks)
                l.Clicked += OnLipstickClicked;

            _dragArea.DragEnded += OnDragEnded;
        }

        private void OnDestroy()
        {
            foreach (var l in _lipsticks)
                l.Clicked -= OnLipstickClicked;

            _dragArea.DragEnded -= OnDragEnded;
        }

        // ── Pickup ────────────────────────────────────────────────────────────

        private void OnLipstickClicked(Lipstick lipstick)
        {
            if (!_hand.Data.CanGrab) return;
            
            new SetHandBusyContext(_hand.Data, true).Execute(); // ← busy from first tween

            if (!UISpaceUtil.RectWorldToHandLocal(
                    lipstick.Rect, _hand, _canvas, out var lipstickHandLocal)) return;

            var pickupTarget  = lipstickHandLocal - _gripOffset;
            var readyPosition = Vector2.Lerp(_hand.RestPosition, pickupTarget, 0.5f);

            _hand.TweenToAnchored(pickupTarget, _pickupTweenDuration)
                .OnComplete(() =>
                {
                    _held = lipstick;
                    lipstick.SetHandCanvas(_hand.GetComponent<Canvas>());
                    lipstick.Rect.SetParent(_hand.transform, worldPositionStays: true);
                    new PickUpLipstickContext(_hand.Data, lipstick.Data).Execute();
                    _hand.TweenToAnchored(readyPosition, _pickupTweenDuration)
                        .OnComplete(() =>
                            new SetHandBusyContext(_hand.Data, false).Execute()); // ← free after ready
                });
        }

        // ── Application ───────────────────────────────────────────────────────

        private void OnDragEnded()
        {
            if (_held is null) return;
            if (!_faceZone.IsHandOver) return;

            new SetHandBusyContext(_hand.Data, true).Execute();
            
            var lipstick = _held;
            _dragArea.enabled = false;

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceLocalPos)) return;

            _hand.TweenToAnchored(faceLocalPos, _applyTweenDuration)
                .OnComplete(() =>
                {
                    new ApplyLipstickContext(_character.Data, lipstick.Index).Execute();

                    if (!UISpaceUtil.WorldToHandLocal(
                            lipstick.ShelfWorldPosition, _hand, _canvas,
                            out var shelfHandLocal)) return;

                    _hand.TweenToAnchored(shelfHandLocal - _gripOffset, _returnDuration)
                        .OnComplete(() =>
                        {
                            new ReturnLipstickContext(_hand.Data, lipstick.Data).Execute();
                            lipstick.Rect.SetParent(
                                lipstick.ShelfParent, worldPositionStays: true);
                            _held = null;

                            _hand.TweenToRest()
                                .OnComplete(() =>
                                {
                                    new SetHandBusyContext(_hand.Data, false).Execute();
                                });
                        });
                });
        }
    }
}