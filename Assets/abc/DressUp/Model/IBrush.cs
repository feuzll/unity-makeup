using abc.DressUp.Interactions;
using PrimeTween;

namespace abc.DressUp.Model
{
    public interface IBrush : ITool
    {
        public void Color(Interaction.ExecutionToken token);
    }
}