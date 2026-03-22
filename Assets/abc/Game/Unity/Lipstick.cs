#nullable enable
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Canvas))]
    public class Lipstick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int _index;

        public int            Index     => _index;
        public Model.Lipstick   Data      { get; } = new();
        public RectTransform  Rect      { get; private set; }
        public Vector3        ShelfWorldPosition { get; private set; }
        public Transform ShelfParent => _shelfParent;

        private Transform _shelfParent;
        private Image     _image;
        private Canvas    _canvas;

        // Raised toward LipstickShelfBehaviour — no direct hand reference needed
        public event System.Action<Lipstick>? Clicked;

        private void Awake()
        {
            Rect        = GetComponent<RectTransform>();
            _image      = GetComponent<Image>();
            _canvas     = GetComponent<Canvas>();
            _shelfParent = transform.parent.parent;
            ShelfWorldPosition = Rect.position;
            _index = transform.GetSiblingIndex();

            Data.StateChanged += OnStateChanged;
        }
        
        private void Start() =>
            StartCoroutine(Unpack());

        private IEnumerator Unpack()
        {
            yield return new WaitForEndOfFrame();

            var corners = new Vector3[4];
            Rect.GetWorldCorners(corners);
            var worldPos    = Rect.position;
            var worldWidth  = Vector3.Distance(corners[0], corners[3]);
            var worldHeight = Vector3.Distance(corners[0], corners[1]);

            Rect.SetParent(_shelfParent, worldPositionStays: false);

            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.pivot     = new Vector2(0.5f, 0.5f);

            var scale      = Rect.lossyScale.x;
            Rect.sizeDelta = new Vector2(worldWidth / scale, worldHeight / scale);
            Rect.position  = worldPos;

            ShelfWorldPosition = worldPos;
        }

        private void OnDestroy() =>
            Data.StateChanged -= OnStateChanged;

        public void OnPointerClick(PointerEventData _)
        {
            if (Data.State != Model.Lipstick.LipstickState.Shelved) return;
            Clicked?.Invoke(this);
        }

        public void SetHandCanvas(Canvas handCanvas)
        {
            _canvas.overrideSorting = true;
            _canvas.sortingOrder    = handCanvas.sortingOrder - 1;
        }

        private void OnStateChanged()
        {
            if (Data.State == Model.Lipstick.LipstickState.Held)
                _image.raycastTarget = false;
            else
            {
                _canvas.overrideSorting = false;
                _image.raycastTarget    = true;
            }
        }
    }
}