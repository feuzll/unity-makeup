#nullable enable
using UnityEngine;

namespace abc.Makeup
{
    public partial class Tool
    {
        public interface IContainer
        {
            public RectTransform Rect { get; }
            public Tool? HeldTool { get; protected set; }
            public void TakeTool(Tool tool)
            {
                tool.Container.HeldTool = null;
                tool.Container = this;
                this.HeldTool =  tool;
            }
        }
    }
}