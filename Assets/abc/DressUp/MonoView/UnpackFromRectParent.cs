using System;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class UnpackFromRectParent : MonoBehaviour
    {
        private RectTransform rect;
        public RectTransform Rect => rect;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        // private void Start() =>
        //     StartCoroutine(Unpack());

        public System.Collections.IEnumerator Unpack()
        {
            yield return new WaitForEndOfFrame();

            var corners     = new Vector3[4];
            Rect.GetWorldCorners(corners);
            var worldPos    = Rect.position;
            var worldWidth  = Vector3.Distance(corners[0], corners[3]);
            var worldHeight = Vector3.Distance(corners[0], corners[1]);

            Rect.SetParent(Rect.parent.parent, worldPositionStays: false);
            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.pivot     = new Vector2(0.5f, 0.5f);

            var scale      = Rect.lossyScale.x;
            Rect.sizeDelta = new Vector2(worldWidth / scale, worldHeight / scale);
            Rect.position  = worldPos;
        }
    }
}