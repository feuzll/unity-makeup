using abc.DressUp.Interactions;
using PrimeTween;

namespace abc.DressUp.Entities
{
    public interface IToolReadyMotionBuilder
    {
        public Sequence BuildAndRunFor(Interaction.ExecutionToken token, ITool tool);
    }
}