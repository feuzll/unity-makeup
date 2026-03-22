#nullable enable
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abc.Game.Unity
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class FaceZone : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler
    {
        private void OnValidate()
        {
            GetComponent<Image>().raycastTarget = true;
        }

        public bool IsHandOver { get; private set; }

        public void OnPointerEnter(PointerEventData _) => IsHandOver = true;
        public void OnPointerExit(PointerEventData  _) => IsHandOver = false;
    }
}