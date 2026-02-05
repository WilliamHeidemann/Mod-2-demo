using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Version_1
{
    public sealed class SegmentGrid
    {
        private readonly HashSet<Segment> _segments = new();

        public int Count => _segments.Count;

        private IEnumerable<Position> GetOccupiedPositions()
        {
            foreach (var segment in _segments)
            {
                foreach (var cellPosition in segment.Positions)
                {
                    yield return cellPosition;
                }
            }
        }

        private IEnumerable<Socket> GetSockets()
        {
            foreach (var segment in _segments)
            {
                foreach (var socket in segment.Sockets)
                {
                    yield return socket;
                }
            }
        }

        /// expects a segment in world position
        public void Add(Segment segment, bool forceAdd = false)
        {
            if (forceAdd || Fits(segment))
            {
                _segments.Add(segment);
            }
        }

        /// expects a segment in world position
        public bool Fits(Segment segment)
        {
            var occupiedPositions = GetOccupiedPositions().ToHashSet();

            foreach (var position in segment.Positions)
            {
                if (occupiedPositions.Contains(position))
                {
                    Debug.LogWarning($"Segment has already been occupied by {position}");
                    return false;
                }
            }

            var existingSockets = GetSockets().ToArray();

            foreach (var socket in segment.Sockets)
            {
                foreach (var other in existingSockets)
                {
                    if (socket.ConnectsTo(other))
                        return true; // just one socket needs to connect
                }
            }
            
            Debug.LogWarning($"No sockets found for {segment}");
            return false;
        }
    }
}