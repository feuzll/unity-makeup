using UnityEngine;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(Image))]
    public class BrushBlurProxy : MonoBehaviour
    {
        [SerializeField] private Brush _source; // the original Brush behaviour

        private Image _image;

        private void Awake() =>
            _image = GetComponent<Image>();

        private void Start()
        {
            // sync immediately in case color was set before we subscribed
            OnColorChanged();
            _source.Data.ColorChanged += OnColorChanged;
        }

        private void OnDestroy() =>
            _source.Data.ColorChanged -= OnColorChanged;

        private void OnColorChanged()
        {
            var c = _source.Data.Color;
            _image.color = new Color(c.R, c.G, c.B, c.A);
        }
    }
}