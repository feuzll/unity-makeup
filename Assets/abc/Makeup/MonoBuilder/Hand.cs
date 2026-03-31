#nullable enable
using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

namespace abc.Makeup.MonoBuilder
{
    [RequireComponent(typeof(RectTransform))]
    public class Hand : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 600;
        [SerializeField] private Canvas handCanvas;
        [SerializeField] private Ease followEase =  Ease.OutQuad;
        private Makeup.Hand? _model;

        public Makeup.Hand Model
        {
            get
            {
                _model ??= new Makeup.Hand(moveSpeed,
                    GetComponent<RectTransform>(), handCanvas, followEase);
                return _model;
            }
        }
    }
}