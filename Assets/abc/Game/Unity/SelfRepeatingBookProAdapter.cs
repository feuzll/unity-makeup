using System;
using UnityEngine;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(BookPro))]
    public class SelfRepeatingBookProAdapter : MonoBehaviour
    {
        [SerializeField] private BookPro book;
        [SerializeField] private float pageFlipTime = 1;
        
        private bool _isPageFlipping = false;

        private void Start()
        {
            
        }

        public void FlipRightPage()
        {
            if (_isPageFlipping) return;
            if (book.CurrentPaper >= book.papers.Length) return;
            _isPageFlipping = true;
            PageFlipper.FlipPage(book, pageFlipTime, 
                FlipMode.RightToLeft, OnFlipComplete);
        }
        public void FlipLeftPage()
        {
            if (_isPageFlipping) return;
            if (book.CurrentPaper <= 0) return;
            _isPageFlipping = true;
            PageFlipper.FlipPage(book, pageFlipTime, 
                FlipMode.LeftToRight, OnFlipComplete);
        }

        private void OnFlipComplete()
        {
            _isPageFlipping = false;
            GuardPolarFlips();
        }
        
        //for now expecting only 2 pair of papers
        private void GuardPolarFlips()
        {
            if (book.CurrentPaper == 1)
                book.CurrentPaper = 3;
            else if (book.CurrentPaper == book.papers.Length - 1)
                book.CurrentPaper = 2;
                
        }
    }
}