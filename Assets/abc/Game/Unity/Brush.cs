using UnityEngine;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    public class Brush : MonoBehaviour
    {
        [SerializeField] private Image _blurCircle; // white soft-circle child Image

        public Model.Brush     Data  { get; } = new();
        public RectTransform Rect  { get; private set; }
        public Vector3       ShelfWorldPosition { get; private set; }
        public Transform     ShelfParent        { get; private set; }

        private Canvas _canvas;

        private void Awake()
        {
            Rect        = GetComponent<RectTransform>();
            _canvas     = GetComponent<Canvas>();
            ShelfParent = transform.parent;

            Data.StateChanged += OnStateChanged;
            Data.ColorChanged += OnColorChanged;
        }

        private void Start() =>
            ShelfWorldPosition = Rect.position;

        private void OnDestroy()
        {
            Data.StateChanged -= OnStateChanged;
            Data.ColorChanged -= OnColorChanged;
        }

        public void SetHandCanvas(Canvas handCanvas)
        {
            _canvas.overrideSorting = true;
            _canvas.sortingOrder    = handCanvas.sortingOrder - 1;
        }

        private void OnStateChanged()
        {
            if (Data.State == Model.Brush.BrushState.Shelved)
                _canvas.overrideSorting = false;
        }

        private void OnColorChanged()
        {
            //Debug.Log($"changing brush color to {Data.Color}");
            var c = Data.Color;
            _blurCircle.color = new UnityEngine.Color(c.R, c.G, c.B, c.A);
        }
    }
}