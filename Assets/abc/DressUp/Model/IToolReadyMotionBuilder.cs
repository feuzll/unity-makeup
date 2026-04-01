using abc.DressUp.Interactions;
using PrimeTween;

namespace abc.DressUp.Model
{
    public interface IToolReadyMotionBuilder
    {
        public Sequence BuildAndRunFor(Interaction.ExecutionToken token, ITool tool);
    }
}