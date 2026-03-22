using UnityEngine;

namespace abc.Game.Unity
{
    public class UISpaceUtil
    {
        /// <summary>
        /// Converts a world-space RectTransform's visual center to
        /// anchored position in the hand container's local space.
        /// </summary>
        public static bool RectWorldToHandLocal(
            RectTransform source,
            Hand hand,
            Canvas canvas,
            out Vector2 result)
        {
            var cam           = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : canvas.worldCamera;
            var centerWorld   = source.TransformPoint(source.rect.center);
            var screenPos     = RectTransformUtility.WorldToScreenPoint(cam, centerWorld);
            return hand.ScreenToLocal(screenPos, out result);
        }
    }
}