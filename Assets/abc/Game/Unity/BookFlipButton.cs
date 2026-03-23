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