using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    public enum FlipDirection { Next, Prev }

    [RequireComponent(typeof(Image))]
    public class BookFlipButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SelfRepeatingBookProAdapter _book;
        [SerializeField] private FlipDirection              _direction;

        [Header("Breathing")]
        [SerializeField] private float _breathScale  = 1.08f;
        [SerializeField] private float _breathPeriod = 1.6f;
        
        private void Start() =>
            Tween.Scale(transform, startValue: 1f, endValue: _breathScale,
                new TweenSettings(_breathPeriod / 2f, Ease.InOutSine, cycles: -1,
                    cycleMode: CycleMode.Yoyo));

        private void OnDestroy() =>
            Tween.StopAll(transform);
        
        public void OnPointerClick(PointerEventData _)
        {
            switch (_direction)
            {
                case FlipDirection.Next: _book.FlipRightPage(); break;
                case FlipDirection.Prev: _book.FlipLeftPage();  break;
            }
        }
    }
}