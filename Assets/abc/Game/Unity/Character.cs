using UnityEngine;

namespace abc.Game.Unity
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private GameObject _acneOverlay; // separate Image layer
        [SerializeField] private GameObject[] _lipColorObjects; // 6 elements, index matches
        [SerializeField] private GameObject[] _eyeshadowColorObjects; // 9 elements, index matches
        
        public Game.Model.Character Data { get; } = new();

        private void Awake()
        {
            Data.SkinChanged += OnSkinChanged;
            Data.LipColorChanged += OnLipColorChanged;
            Data.EyeColorChanged += OnEyeColorChanged;
        }
        

        private void OnDestroy()
        {
            Data.SkinChanged -= OnSkinChanged;
            Data.LipColorChanged -= OnLipColorChanged;
            Data.EyeColorChanged -= OnEyeColorChanged;
        }

        private void OnSkinChanged() =>
            _acneOverlay.SetActive(Data.HasAcne);
        
        private void OnLipColorChanged()
        {
            for (int i = 0; i < _lipColorObjects.Length; i++)
                _lipColorObjects[i].SetActive(i == Data.LipColorIndex);
        }
        private void OnEyeColorChanged()
        {
            for (int i = 0; i < _eyeshadowColorObjects.Length; i++)
                _eyeshadowColorObjects[i].SetActive(i == Data.EyeColorIndex);
        }
    }
}