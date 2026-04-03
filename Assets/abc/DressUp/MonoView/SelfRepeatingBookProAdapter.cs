#nullable enable
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class SelfRepeatingBookProAdapter : MonoBehaviour
    {
        [SerializeField] private BookPro book;
        [SerializeField] private float pageFlipTime = 1;
        
        private bool _isPageFlipping = false;
        public bool IsFlipping => _isPageFlipping;
        public event System.Action<bool>? FlippingChanged; // true = started, false = finished
        

        public void FlipRightPage()
        {
            if (_isPageFlipping) return;
            if (book.CurrentPaper >= book.papers.Length) return;
            _isPageFlipping = true;
            FlippingChanged?.Invoke(true);
            PageFlipper.FlipPage(book, pageFlipTime, 
                FlipMode.RightToLeft, OnFlipComplete);
        }
        public void FlipLeftPage()
        {
            if (_isPageFlipping) return;
            if (book.CurrentPaper <= 0) return;
            _isPageFlipping = true;
            FlippingChanged?.Invoke(true);
            PageFlipper.FlipPage(book, pageFlipTime, 
                FlipMode.LeftToRight, OnFlipComplete);
        }

        private void OnFlipComplete()
        {
            _isPageFlipping = false;
            GuardPolarFlips();
            FlippingChanged?.Invoke(false);
        }
        
        private void GuardPolarFlips()
        {
            if (book.CurrentPaper == 1)
                book.CurrentPaper = book.papers.Length - 2;
            else if (book.CurrentPaper == book.papers.Length - 1)
                book.CurrentPaper = 2;
                
        }
    }
}