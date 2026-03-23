using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace abc.Game.Unity
{
    public class ParticlePlayer : MonoBehaviour
    {
        [SerializeField] private UIParticle _uiParticle;

        private RectTransform _rectTransform;

        private void Awake() =>
            _rectTransform = _uiParticle.GetComponent<RectTransform>();

        public void PlayAt(Vector2 anchoredPosition)
        {
            _rectTransform.anchoredPosition = anchoredPosition;
            _uiParticle.Stop();
            _uiParticle.Play();
        }
    }
}