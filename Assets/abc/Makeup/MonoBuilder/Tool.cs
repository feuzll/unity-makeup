#nullable enable
using System;
using System.ComponentModel;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Makeup.MonoBuilder
{
    [RequireComponent(typeof(Image))]
    public  class Tool : MonoBehaviour, IPointerClickHandler, Makeup.Tool.IInput
    {
        private Makeup.Tool? _model;
        [SerializeField] private ToolSlot initialContainer;
        [SerializeField] private Makeup.Tool.HandReadyType handReady;
        [SerializeField] private ShakeSettings applySettings;
        
        public Makeup.Tool Model
        {
            get
            {
                var rect =  GetComponent<RectTransform>();
                _model ??= new Makeup.Tool(
                    rect,  GetComponent<Image>(), 
                    initialContainer, handReady, applySettings);
                return _model;
            }
            private set => _model = value;
        }

        private void Awake()
        {
            GetComponent<Image>().raycastTarget = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked {gameObject.name} tool");
            (this as Makeup.Tool.IInput).RaiseOnClick(Model);
        }
    }
}