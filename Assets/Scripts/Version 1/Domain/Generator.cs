using System;
using System.Collections.Generic;
using System.Linq;
using UtilityToolkit.Runtime;
using Random = UnityEngine.Random;

namespace Version_1.Domain
{
    public static class Generator
    {
        public static Segment CenterCube => new()
        {
            Positions = new[] { Position.Center },
            Sockets = new[] { new Socket(Position.Center, Direction.Up, Archetype.Blue) },
            Pivot = Position.Center
        };

        public static Segment Stick => new()
        {
            Positions = new[]
            {
                Position.Center,
                Position.Center + Direction.Right
            },
            Sockets = new[]
            {
                new Socket(Position.Center + Direction.Right, Direction.Up, Archetype.Blue)
            },
            Pivot = Position.Center
        };

        public static Segment Square => new()
        {
            Positions = new[]
            {
                new Position(-1, 0, -1),
                new Position(0, 0, -1),
                new Position(1, 0, -1),

                new Position(-1, 0, 0),
                new Position(1, 0, 0),

                new Position(-1, 0, 1),
                new Position(0, 0, 1),
                new Position(1, 0, 1)
            },
            Sockets =  new[]
            {
                new Socket(new Position(-1, 0, -1), Direction.Up, Archetype.Blue),
                new Socket(new Position(1, 0, -1), Direction.Up, Archetype.Red),
                new Socket(new Position(-1, 0, 1), Direction.Up, Archetype.Yellow),
                new Socket(new Position(1, 0, 1), Direction.Up, Archetype.Green),
            },
            Pivot = Position.Center
        };

        public static Segment Generate()
        {
            HashSet<Position> positions = new();
            Position current = new(0, 0, 0);

            positions.Add(current);
            int numberOfCells = Random.Range(1, 5);

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
                    Archetype archetype = Enum.GetValues(typeof(Archetype)).Cast<Archetype>().RandomElement();
                    Socket socket = new Socket(cell, direction, archetype);
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