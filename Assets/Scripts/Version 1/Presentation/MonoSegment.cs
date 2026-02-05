using System;
using UnityEngine;

namespace Version_1.Presentation
{
    public class MonoSegment : MonoBehaviour
    {
        public Segment Model { get; private set; }

        private void Awake()
        {
            Model = new Segment
            {
                Positions = new[] { new Position(0, 0, 0) },
                Sockets = new[]
                {
                    new Socket
                    {
                        Position = new Position(0, 0, 0),
                        Direction = Direction.Up,
                        Archetype = Archetype.Blue
                    },
                    new Socket
                    {
                        Position = new Position(0, 0, 0),
                        Direction = Direction.Down,
                        Archetype = Archetype.Blue
                    }
                }
            };
        }
    }
}