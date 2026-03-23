#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class BrushColorPicker : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Color _fallbackColor = Color.white;

        public RectTransform Rect   { get; private set; }
        public Color         Color  { get; private set; }
        public int           Index  { get; private set; }
        public Transform     ShelfParent      { get; private set; }
        public Vector3       ShelfWorldPosition { get; private set; }

        public event System.Action<BrushColorPicker>? Clicked;

        private Image _image;

        private void Awake()
        {
            Rect        = GetComponent<RectTransform>();
            _image      = GetComponent<Image>();
            ShelfParent = transform.parent.parent;
            Index       = transform.GetSiblingIndex();

            // sample center pixel from sprite, fall back to inspector color
            Color = SampleCenterColor(_image.sprite) ?? _fallbackColor;
        }

        private void Start() =>
            StartCoroutine(Unpack());

        private System.Collections.IEnumerator Unpack()
        {
            yield return new WaitForEndOfFrame();

            var corners     = new Vector3[4];
            Rect.GetWorldCorners(corners);
            var worldPos    = Rect.position;
            var worldWidth  = Vector3.Distance(corners[0], corners[3]);
            var worldHeight = Vector3.Distance(corners[0], corners[1]);

            Rect.SetParent(ShelfParent, worldPositionStays: false);
            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.pivot     = new Vector2(0.5f, 0.5f);

            var scale      = Rect.lossyScale.x;
            Rect.sizeDelta = new Vector2(worldWidth / scale, worldHeight / scale);
            Rect.position  = worldPos;

            ShelfWorldPosition = worldPos;
        }

        private static Color? SampleCenterColor(Sprite sprite)
        {
            if (sprite == null) return null;

            var texture = sprite.texture;
            if (!texture.isReadable) return null;

            // center of the sprite in texture space
            var rect    = sprite.textureRect;
            var centerX = (int)(rect.x + rect.width  * 0.5f);
            var centerY = (int)(rect.y + rect.height * 0.5f);

            return texture.GetPixel(centerX, centerY);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this);
        }
    }
}