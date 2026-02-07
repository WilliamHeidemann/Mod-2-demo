using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Version_1.Domain
{
    public static class Generator
    {
        public static Segment GenerateBox()
        {
            var center = new Position(0, 0, 0);
            return new Segment
            {
                Positions = new[] { new Position(0, 0, 0) },
                Sockets = new Socket[]
                {
                    new(center, Direction.Up,  Archetype.Blue),
                    new(center, Direction.Down,  Archetype.Blue),
                    new(center, Direction.Right,  Archetype.Blue),
                    new(center, Direction.Left,  Archetype.Blue),
                    new(center, Direction.Front,  Archetype.Blue),
                    new(center, Direction.Back,  Archetype.Blue),
                },
            };
        }
        
        public static Segment Generate()
        {
            HashSet<Position> positions = new();
            Position current = new(0, 0, 0);

            positions.Add(current);
            var numberOfCells = Random.Range(1, 6);

            for (int i = 0; i < numberOfCells; i++)
            {
                current += Direction.All.RandomElement();
                if (!positions.Contains(current))
                {
                    positions.Add(current);
                }
                else
                {
                    i--;
                }
            }

            int numberOfSockets = Random.Range(5, 10);
            List<Socket> sockets = new();
            for (int i = 0; i < numberOfSockets; i++)
            {
                var cell = positions.RandomElement();
                var direction = Direction.All.RandomElement();
                if (!positions.Contains(cell + direction))
                {
                    var socket = new Socket(cell, direction, Archetype.Blue);
                    sockets.Add(socket);
                }
                else
                {
                    i--;
                }
            }

            return new Segment()
            {
                Positions = positions.ToArray(),
                Sockets = sockets.ToArray(),
            };
        }
    }
}