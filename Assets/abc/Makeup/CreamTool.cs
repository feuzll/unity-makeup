#nullable enable
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace abc.Makeup
{
    public class CreamTool : Tool
    {
        public CreamTool(RectTransform rect, Graphic graph, 
            IContainer container, HandReadyType handReady, ShakeSettings applySettings) 
            : base(rect, graph, container, handReady, applySettings) {}
    }
}