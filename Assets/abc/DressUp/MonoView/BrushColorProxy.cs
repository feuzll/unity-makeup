using System;
using UnityEngine;
using UnityEngine.UI;

namespace abc.DressUp.MonoView
{
    [RequireComponent(typeof(Image))]
    public class BrushColorProxy : MonoBehaviour
    {
        [SerializeField] private Brush brush;
        private Image _image;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            brush.ColorChanged += color => _image.color = color;
        }
    }
}