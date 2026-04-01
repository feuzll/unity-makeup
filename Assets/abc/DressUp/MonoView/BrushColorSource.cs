#nullable enable
using System;
using abc.DressUp.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.DressUp.MonoView
{
    [RequireComponent(typeof(Image))]
    public class BrushColorSource : MonoBehaviour, 
        IBrush.IColorSource, IPointerClickHandler
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private ICharacter.ViewState viewState;
        [SerializeField] private Color color;
        [SerializeField] private Image image;
        [SerializeField] private Transform page;
        [SerializeField] private Color fallbackColor = Color.deepPink;

        public Color Color => color;

        public RectTransform Rect => rect;

        public ICharacter.ViewState ViewState => viewState;

        public event Action? Clicked;
        
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            page = transform.parent.parent;
            viewState.value = transform.GetSiblingIndex();

            // sample center pixel from sprite, fall back to inspector color
            color = SampleCenterColor(image.sprite) ?? fallbackColor;
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

            Rect.SetParent(page, worldPositionStays: false);
            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.pivot     = new Vector2(0.5f, 0.5f);

            var scale      = Rect.lossyScale.x;
            Rect.sizeDelta = new Vector2(worldWidth / scale, worldHeight / scale);
            Rect.position  = worldPos;
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
            Debug.Log("clicked brush source");
            Clicked?.Invoke();
        }
        
    }
}