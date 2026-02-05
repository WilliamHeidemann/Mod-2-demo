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
                    new Socket(new Position(0, 0, 0), Direction.Up, Archetype.Blue),
                    new Socket(new Position(0, 0, 0), Direction.Down, Archetype.Blue),
                }
            };
        }
    }
}