using System;
using UnityEngine;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    public class BookPagesInteractables : MonoBehaviour
    {
        [SerializeField] private SelfRepeatingBookProAdapter _book;
        [SerializeField] private Image[] _interactables;

        private void Awake()
        {
            // collect Image components from all book-page interactable types
            var lipsticks    = _book.GetComponentsInChildren<Lipstick>();
            var colorSquares = _book.GetComponentsInChildren<BrushColorPicker>();
            var brushes      = _book.GetComponentsInChildren<Brush>();

            var images = new System.Collections.Generic.List<Image>();

            foreach (var l  in lipsticks)    images.Add(l.GetComponent<Image>());
            foreach (var sq in colorSquares) images.Add(sq.GetComponent<Image>());
            foreach (var b  in brushes)      images.Add(b.GetComponent<Image>());

            _interactables = images.ToArray();
        }

        private void Start()
        {
            _book.FlippingChanged += OnFlippingChanged;
            // sync in case flipping was already in progress
            OnFlippingChanged(_book.IsFlipping);
        }

        private void OnDestroy() =>
            _book.FlippingChanged -= OnFlippingChanged;

        private void OnFlippingChanged(bool isFlipping)
        {
            foreach (var image in _interactables)
                image.raycastTarget = !isFlipping;
        }
    }
}