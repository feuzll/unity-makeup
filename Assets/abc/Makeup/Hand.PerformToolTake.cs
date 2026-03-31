using PrimeTween;

namespace abc.Makeup
{
    public partial class Hand
    {
        public Sequence PerformToolTake(Tool tool)
        {
            return Sequence.Create()
                .Chain(MoveToTarget(tool.Rect))
                .ChainCallback(() => (this as Tool.IContainer).TakeTool(tool));
        }
    }
}