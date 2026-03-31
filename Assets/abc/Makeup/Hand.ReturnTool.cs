using PrimeTween;

namespace abc.Makeup
{
    public partial class Hand
    {
        public Sequence ReturnTool(Tool tool)
        {
            var worldInSlotPosition = tool.InitialContainer.Rect.TransformPoint(tool.SlotPosition);
            return Sequence.Create()
                .Chain(MoveToWorld(worldInSlotPosition))
                .ChainCallback(() => tool.InitialContainer.TakeTool(tool));
        }
    }
}