using UnityEngine;

namespace abc.Game.Unity
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private GameObject _acneOverlay; // separate Image layer

        public Game.Model.Character Data { get; } = new();

        private void Awake() =>
            Data.SkinChanged += OnSkinChanged;

        private void OnDestroy() =>
            Data.SkinChanged -= OnSkinChanged;

        private void OnSkinChanged() =>
            _acneOverlay.SetActive(Data.HasAcne);
    }
}