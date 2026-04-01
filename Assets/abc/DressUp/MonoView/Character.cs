using System;
using System.Collections.Generic;
using abc.DressUp.Entities;
using abc.DressUp.Interactions;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] private GameObject acne;
        [SerializeField] private List<GameObject> eyeShadows;
        
        public void ApplyViewState(Interaction.ExecutionToken token, ICharacter.ViewState viewState)
        {
            Debug.Log($"ApplyViewState with index {viewState.value}");
            switch (viewState.type)
            {
                case ICharacter.ViewState.Type.Lips:
                    break;
                case ICharacter.ViewState.Type.Eyes:
                    for (int i = 0; i < eyeShadows.Count; i++)
                    {
                        eyeShadows[i].SetActive(viewState.value == i);
                    }
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