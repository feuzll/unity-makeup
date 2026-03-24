#nullable enable
using abc.Game.Contexts;
using abc.Game.Model;
using PrimeTween;
using Tween = PrimeTween.Tween;
using UnityEngine;

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
        [SerializeField] private Vector2 _colorPickOffset  = new Vector2(0, -130f); // eyes/blush placement

        private BrushColorPicker? _pendingSquare; // selected before brush is grabbed
        private RectTransform _handRect;
        
        private void Start()
        {
            _handRect = _hand.GetComponent<RectTransform>();
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

            if (!UISpaceUtil.RectWorldToHandLocal(_brush.Rect, _hand, _canvas, out var brushHandLocal)) return;
            if (!UISpaceUtil.RectWorldToHandLocal((RectTransform)_faceZone.transform, _hand, _canvas, out var faceHandLocal)) return;
            if (!UISpaceUtil.RectWorldToHandLocal(square.Rect, _hand, _canvas, out var squareHandLocal)) return;

            var pickupTarget  = brushHandLocal - _gripOffset;
            var colorTarget   = squareHandLocal + _colorPickOffset;
            var readyPosition = Vector2.Lerp(squareHandLocal, faceHandLocal, 0.5f);

            Sequence.Create()
                .Chain(_hand.TweenToAnchored(pickupTarget))
                .ChainCallback(() => GrabBrush(square))
                .Chain(_hand.TweenToAnchored(pickupTarget, colorTarget))
                .ChainCallback(() => ColorBrush(square))
                .Chain(Tween.ShakeLocalPosition(_handRect,
                    new Vector3(_shakeStrength, 0f, 0f), _shakeDuration))
                .Chain(_hand.TweenToAnchored(colorTarget, readyPosition))
                .ChainCallback(() => new SetHandBusyContext(_hand.Data, false).Execute());
        }

        // ── Application ───────────────────────────────────────────────────────

        private void OnDragEnded(Vector2 screenPos)
        {
            if (_brush.Data.State != Model.Brush.BrushState.Held) return;
            if (_hand.Data.IsBusy) return;
            if (!_faceZone.ContainsScreenPoint(screenPos)) return;
            StartApplySequence();
        }

        private void OnFaceClicked()
        {
            if (_brush.Data.State != Model.Brush.BrushState.Held) return;
            if (_hand.Data.IsBusy) return;
            StartApplySequence();
        }

        private void StartApplySequence()
        {
            new SetHandBusyContext(_hand.Data, true).Execute();

            if (!UISpaceUtil.RectWorldToHandLocal(
                    (RectTransform)_faceZone.transform, _hand, _canvas,
                    out var faceLocalPos)) return;

            if (!UISpaceUtil.WorldToHandLocal(
                    _brush.ShelfWorldPosition, _hand, _canvas,
                    out var shelfHandLocal)) return;
            
            var faceTarget   = faceLocalPos + _faceOffset;
            var returnTarget = shelfHandLocal - _gripOffset;

            Sequence.Create()
                .Chain(_hand.TweenToAnchored(faceTarget))
                .Chain(Tween.ShakeLocalPosition(_handRect,
                    new Vector3(_shakeStrength, _shakeStrength, 0f), _shakeDuration))
                .ChainCallback(ApplyFaceColor)
                .Chain(_hand.TweenToAnchored(faceTarget, returnTarget))
                .ChainCallback(ReturnBrush)
                .Chain(_hand.TweenToRest())
                .ChainCallback(() => new SetHandBusyContext(_hand.Data, false).Execute());
        }
        
        private void ApplyFaceColor()
        {
            if (_pendingSquare is null) return;
            switch (_brushType)
            {
                case BrushType.Eyes:
                    new ApplyEyeColorContext(_character.Data, _pendingSquare.Index).Execute();
                    break;
                case BrushType.Blush:
                    new ApplyBlushColorContext(_character.Data, _pendingSquare.Index).Execute();
                    break;
            }
        }
        
        private void ColorBrush(BrushColorPicker square)
        {
            var c = square.Color;
            new ColorBrushContext(_brush.Data, new BrushColor(c.r, c.g, c.b, c.a)).Execute();
        }
        
        private void GrabBrush(BrushColorPicker square)
        {
            _brush.SetHandCanvas(_hand.GetComponent<Canvas>());
            _brush.Rect.SetParent(_hand.transform, worldPositionStays: true);
            new PickUpBrushContext(_hand.Data, _brush.Data).Execute();
        }
        
        private void ReturnBrush()
        {
            new ReturnBrushContext(_hand.Data, _brush.Data).Execute();
            _brush.Rect.SetParent(_brush.ShelfParent, worldPositionStays: true);
            _pendingSquare = null;
        }
    }
}