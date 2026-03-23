using UnityEngine;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    public class BookFlipButtons : MonoBehaviour
    {
        [SerializeField] private Hand                      _hand;
        [SerializeField] private SelfRepeatingBookProAdapter _book;
        [SerializeField] private Image _nextButton;
        [SerializeField] private Image _prevButton;

        private void Start()
        {
            _hand.Data.HeldToolChanged += Refresh;
            Refresh();
        }

        private void OnDestroy() =>
            _hand.Data.HeldToolChanged -= Refresh;

        private void Refresh()
        {
            var canFlip = _hand.Data.CanGrab;
            _nextButton.gameObject.SetActive(canFlip);
            _prevButton.gameObject.SetActive(canFlip);
        }
    }
}