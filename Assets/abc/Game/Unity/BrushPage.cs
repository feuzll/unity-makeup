#nullable enable
using abc.Game.Contexts;
using abc.Game.Model;
using UnityEngine;
using Color = System.Drawing.Color;

namespace abc.Game.Unity
{
    public enum BrushType { Eyes, Blush }
    public class BrushPage : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Hand      _hand;
        [SerializeField] private DragArea   _dragArea;
        [SerializeField] private Character  _character;
        [SerializeField] private FaceZone   _faceZone;
        [SerializeField] private Canvas              _canvas;
        [SerializeField] private Brush      _brush;
        [SerializeField] private BrushColorPicker[] _colorSquares;

        [Header("Config")]
        [SerializeField] private BrushType _brushType;

        [Header("Timing")]
        [SerializeField] private float _pickupTweenDuration  = 0.35f;
        [SerializeField] private float _toColorTweenDuration = 0.3f;
        [SerializeField] private float _applyTweenDuration   = 0.3f;
        [SerializeField] private float _returnDuration       = 0.3f;
        [SerializeField] private float _shakeDuration        = 0.4f;
        [SerializeField] private float _shakeStrength        = 18f;

        [Header("Grip")]
        [SerializeField] private Vector2 _gripOffset  = new Vector2(0, -20f);
        [SerializeField] private Vector2 _faceOffset  = new Vector2(0, -20f); // eyes/blush placement

        private BrushColorPicker? _pendingSquare; // selected before brush is grabbed

        private void Start()
        {
            foreach (var sq in _colorSquares)
                sq.Clicked += OnColorSquareClicked;

            _dragArea.DragEnded     += OnDragEnded;
            _faceZone.ClickedOnFace += OnFaceClicked;
        }

        private void OnDestroy()
        {
            foreach (var sq in _colorSquares)
                sq.Clicked -= OnColorSquareClicked;

            _dragArea.DragEnded     -= OnDragEnded;
            _faceZone.ClickedOnFace -= OnFaceClicked;
        }

        // ── Pickup + color ────────────────────────────────────────────────────

        private void OnColorSquareClicked(BrushColorPicker square)
        {
            if (!_hand.Data.CanGrab) return;

            _pendingSquare = square;
            new SetHandBusyContext(_hand.Data, true).Execute();

            // 1. hand → brush
            if (!UISpaceUtil.RectWorldToHandLocal(
                    _brush.Rect, _hand, _canvas, out var brushHandLocal)) return;
            
            // face center in hand local space
            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceHandLocal)) return;

            if (!UISpaceUtil.RectWorldToHandLocal(
                    square.Rect, _hand, _canvas, out var squareHandLocal)) return;
            
            var pickupTarget  = brushHandLocal - _gripOffset;
            var readyPosition = Vector2.Lerp(squareHandLocal, faceHandLocal, 0.5f);

            _hand.TweenToAnchored(pickupTarget, _pickupTweenDuration)
                .OnComplete(() =>
                {
                    _brush.SetHandCanvas(_hand.GetComponent<Canvas>());
                    _brush.Rect.SetParent(_hand.transform, worldPositionStays: true);
                    new PickUpBrushContext(_hand.Data, _brush.Data).Execute();

                    _hand.TweenToAnchored(squareHandLocal, _toColorTweenDuration)
                        .OnComplete(() =>
                        {
                            // 3. color brush
                            var dataColor = UISpaceUtil.ConvertUnityToSystemDrawingColor(square.Color);
                            var c     = square.Color; // UnityEngine.Color
                            var brushColor = new BrushColor(c.r, c.g, c.b, c.a);
                            new ColorBrushContext(_brush.Data, brushColor).Execute();

                            // 4. move to ready position
                            _hand.TweenToAnchored(readyPosition, _pickupTweenDuration)
                                .OnComplete(() =>
                                    new SetHandBusyContext(_hand.Data, false).Execute());
                        });
                });
        }

        // ── Application ───────────────────────────────────────────────────────

        private void OnDragEnded(Vector2 screenPos)
        {
            if (_brush.Data.State != Model.Brush.BrushState.Held) return;
            if (!_faceZone.ContainsScreenPoint(screenPos)) return;
            StartApplySequence();
        }

        private void OnFaceClicked()
        {
            if (_brush.Data.State != Model.Brush.BrushState.Held) return;
            StartApplySequence();
        }

        private void StartApplySequence()
        {
            new SetHandBusyContext(_hand.Data, true).Execute();

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceLocalPos)) return;

            var targetPos = faceLocalPos + _faceOffset;

            _hand.TweenToAnchored(targetPos, _applyTweenDuration)
                .OnComplete(() =>
                {
                    PrimeTween.Tween.ShakeLocalPosition(
                            _hand.GetComponent<RectTransform>(),
                            strength: new Vector3(_shakeStrength, _shakeStrength, 0f),
                            _shakeDuration)
                        .OnComplete(() =>
                        {
                            // apply face color
                            if (_pendingSquare is not null)
                            {
                                switch (_brushType)
                                {
                                    case BrushType.Eyes:
                                        new ApplyEyeColorContext(
                                            _character.Data, _pendingSquare.Index).Execute();
                                        break;
                                    case BrushType.Blush:
                                        new ApplyBlushColorContext(
                                            _character.Data, _pendingSquare.Index).Execute();
                                        break;
                                }
                            }

                            if (!UISpaceUtil.WorldToHandLocal(
                                    _brush.ShelfWorldPosition, _hand, _canvas,
                                    out var shelfHandLocal)) return;

                            _hand.TweenToAnchored(shelfHandLocal - _gripOffset, _returnDuration)
                                .OnComplete(() =>
                                {
                                    new ReturnBrushContext(_hand.Data, _brush.Data).Execute();
                                    _brush.Rect.SetParent(
                                        _brush.ShelfParent, worldPositionStays: true);
                                    _pendingSquare = null;

                                    _hand.TweenToRest()
                                        .OnComplete(() =>
                                            new SetHandBusyContext(_hand.Data, false).Execute());
                                });
                        });
                });
        }
    }
}