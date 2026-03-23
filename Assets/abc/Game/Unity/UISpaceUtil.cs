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
        
        public static bool WorldToHandLocal(
            Vector3 worldPos,
            Hand hand,
            Canvas canvas,
            out Vector2 result)
        {
            var cam       = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : canvas.worldCamera;
            var screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
            return hand.ScreenToLocal(screenPos, out result);
        }
        
        public static System.Drawing.Color ConvertUnityToSystemDrawingColor(UnityEngine.Color unityColor)
        {
            // Convert float values (0f to 1f) to byte values (0 to 255)
            int r = Mathf.RoundToInt(unityColor.r * 255f);
            int g = Mathf.RoundToInt(unityColor.g * 255f);
            int b = Mathf.RoundToInt(unityColor.b * 255f);
            int a = Mathf.RoundToInt(unityColor.a * 255f);

            // Create the System.Drawing.Color object using the FromArgb method
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
        
        public static UnityEngine.Color DrawingColorToUnityColor(System.Drawing.Color drawingColor)
        {
            return new UnityEngine.Color(
                drawingColor.R / 255.0f,
                drawingColor.G / 255.0f,
                drawingColor.B / 255.0f,
                drawingColor.A / 255.0f
            );
        }
    }
}