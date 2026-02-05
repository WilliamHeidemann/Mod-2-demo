using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Version_1.Presentation
{
    public class MonoSegment : MonoBehaviour
    {
        private void Awake()
        {
            var segment = ReadSegment();
            print($"Name: {name} Positions: {segment.Positions.Count} : Sockets: {segment.Sockets.Count}");
        }

        private Segment ReadSegment()
        {
            var monoSockets = GetComponentsInChildren<MonoSocket>();
            var sockets = new Socket[monoSockets.Length];
            for (var i = 0; i < monoSockets.Length; i++)
            {
                var monoSocket = monoSockets[i];
                sockets[i] = new Socket(monoSocket.transform.position.ToPosition(),
                    monoSocket.transform.forward.ToPosition().AsDirection(), monoSocket.archetype);
            }

            var positions = ReadPositions();

            return new Segment
            {
                Sockets = sockets,
                Positions = positions.ToList()
            };
        }

        private IEnumerable<Position> ReadPositions()
        {
            var colliders = GetComponentsInChildren<Collider>();
            var bounds = CalculateBounds(colliders);

            foreach (var cell in IterateGrid(bounds))
            {
                if (Physics.CheckBox(cell, Vector3.one * .5f, Quaternion.identity)) // , LayerMask.GetMask("Segment")
                {
                    yield return cell.ToPosition();
                }
            }
        }

        private static Bounds CalculateBounds(Collider[] colliders)
        {
            if (colliders == null || colliders.Length == 0)
                throw new ArgumentException("No colliders supplied to CalculateBounds");

            var bounds = colliders[0].bounds;

            for (int i = 1; i < colliders.Length; i++)
            {
                bounds.Encapsulate(colliders[i].bounds);
            }

            return bounds;
        }

        private static IEnumerable<Vector3> IterateGrid(Bounds bounds)
        {
            var min = bounds.min.ToVector3Int();
            var max = bounds.max.ToVector3Int();

            for (int x = min.x; x <= max.x; x++)
            for (int y = min.y; y <= max.y; y++)
            for (int z = min.z; z <= max.z; z++)
                yield return new Vector3(x,y,z);
        }

        public Segment Model { get; private set; } = new Segment
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