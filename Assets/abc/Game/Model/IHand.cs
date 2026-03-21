#nullable enable
using System;
using abc.Game.Unity;

namespace abc.Game.Model
{
    public partial interface IHand
    {
        /*protected void TryMoveTo(float x, float y);

        protected ITool? Tool { get; set; }

        protected Action ActiveToolChanged { get; }*/

        /*protected void TryGrab(ITool tool)
        {
            if (Tool is not null) return;
            TryMoveTo(tool.Position.x, tool.Position.y);
            tool.BindTo(this);
            Tool = tool;
            ActiveToolChanged?.Invoke();
        }

        protected void TryDrop(ITool tool);
        
        protected void TryUseActiveTool()
        {
            if (Tool is null) return;
            Tool.PerformJob();
            ActiveToolChanged?.Invoke();
        }

        public interface IDragger
        {
            protected IHand Target { get; }

            public void TryDragTo(float x, float y)
            {
                if (Target.Tool is not null)
                    Target.TryMoveTo(x, y);
            }
        }*/
        
    }
}