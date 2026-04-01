#nullable enable
using abc.DressUp.Interactions;
using UnityEngine;

namespace abc.DressUp.Entities
{
    public partial interface ITool
    {
        public interface IContainer
        {
            public RectTransform Rect { get; }
            public ITool? HeldTool { get;}

            protected void SetHeldTool(ITool? tool);

            public void TakeTool(Interaction.ExecutionToken token, ITool tool)
            {
                tool.Container.SetHeldTool(null);
                tool.Container = this;
                this.SetHeldTool(tool);
            }
        }
    }
}