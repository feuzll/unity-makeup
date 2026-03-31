#nullable enable
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace abc.Makeup.MonoBuilder
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Hand handHandScope = null!;
        [SerializeField] private Tool[] tools = null!;
        [SerializeField] private FaceZone faceZone;
        private Makeup.Room? _room;

        public Makeup.Room Model
        {
            get
            {
                if (_room is not null) return _room;
                var toolModels = new Makeup.Tool[tools.Length];
                for (var index = 0; index < tools.Length; index++)
                {
                    var tool = tools[index];
                    toolModels[index] = tool.Model;
                }
                _room = new Makeup.Room(toolModels, handHandScope.Model, faceZone);

                return _room;
            }
            private set => _room = value;
        }
    }
}