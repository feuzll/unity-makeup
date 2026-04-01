using System;
using abc.DressUp.Entities;
using abc.DressUp.Interactions;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] private GameObject acne;
        
        public void ApplyViewState(Interaction.ExecutionToken token, ICharacter.ViewState viewState)
        {
            switch (viewState.type)
            {
                case ICharacter.ViewState.Type.Lips:
                    break;
                case ICharacter.ViewState.Type.Eyes:
                    break;
                case ICharacter.ViewState.Type.Blush:
                    break;
                case ICharacter.ViewState.Type.Acne:
                    acne.SetActive(viewState.value >= 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}