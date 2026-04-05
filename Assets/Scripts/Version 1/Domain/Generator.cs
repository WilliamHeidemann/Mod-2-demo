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
            return new Segment
            {
                Positions = new[] { Position.Center },
                Sockets = new Socket[]
                {
                    new(Position.Center, Direction.Up, Archetype.Blue),
                    new(Position.Center, Direction.Down, Archetype.Blue),
                    new(Position.Center, Direction.Right, Archetype.Blue),
                    new(Position.Center, Direction.Left, Archetype.Blue),
                    new(Position.Center, Direction.Front, Archetype.Blue),
                    new(Position.Center, Direction.Back, Archetype.Blue),
                },
            };
        }

        public static Segment GenerateStick()
        {
            return new()
            {
                Positions = new[]
                {
                    Position.Center,
                    Position.Center + Direction.Right
                },
                Sockets = new[]
                {
                    new Socket(Position.Center + Direction.Right, Direction.Up, Archetype.Blue)
                }
            };
        }

        public static Segment Generate()
        {
            HashSet<Position> positions = new();
            Position current = new(0, 0, 0);

            positions.Add(current);
            int numberOfCells = Random.Range(1, 3);

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

            int numberOfSockets = Random.Range(2, 6);
            List<Socket> sockets = new();
            for (int i = 0; i < numberOfSockets; i++)
            {
                Position cell = positions.RandomElement();
                Direction direction = Direction.All.RandomElement();
                if (!positions.Contains(cell + direction))
                {
                    Socket socket = new Socket(cell, direction, Archetype.Blue);
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