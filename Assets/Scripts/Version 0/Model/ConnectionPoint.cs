using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class ConnectionPoint
    {
        [SerializeField] private Archetype archetype;
        [SerializeField] private Direction direction;
        [SerializeField] private Vector3Int position;

        public ConnectionPoint(Vector3Int vector3Int, Direction argDirection, Archetype argArchetype)
        {
            position = vector3Int;
            direction = argDirection;
            archetype = argArchetype;
        }

        public Archetype Archetype => archetype;
        public Direction Direction => direction;
        public Vector3Int Position => position;
    }
}