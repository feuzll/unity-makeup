using System;
using abc.DressUp.Entities;
using UnityEngine;

namespace abc.DressUp.MonoView
{
    public class Game : MonoBehaviour
    {
        [SerializeField] public ITool.IContainer hand;

        private void Awake()
        {
            
        }
    }
}