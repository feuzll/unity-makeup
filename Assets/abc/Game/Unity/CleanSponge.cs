using abc.Game.Contexts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(Image))]
    public class CleanSponge : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Character _character;

        public void OnPointerClick(PointerEventData _) =>
            new CleanMakeupContext(_character.Data).Execute();
    }
}