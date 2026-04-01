using abc.DressUp.Interactions;

namespace abc.DressUp.Entities
{
    public partial interface ICharacter
    {
        public void ApplyViewState(Interaction.ExecutionToken token, ViewState viewState);
    }
}