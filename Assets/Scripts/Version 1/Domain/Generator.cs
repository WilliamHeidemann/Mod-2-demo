using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Version_1.Domain
{
    public static class Generator
    {
        public static Segment Generate()
        {
            HashSet<Position> positions = new();
            Position current = new Position(0, 0, 0);

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

            int numberOfSockets = Random.Range(2, 4);
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