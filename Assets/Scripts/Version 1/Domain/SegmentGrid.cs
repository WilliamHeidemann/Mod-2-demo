using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Version_1
{
    public sealed class SegmentGrid
    {
        private readonly HashSet<Segment> _segments = new();

        public int Count => _segments.Count;

        public IEnumerable<Position> GetOccupiedPositions()
        {
            foreach (Segment segment in _segments)
            {
                foreach (Position cellPosition in segment.Positions)
                {
                    yield return cellPosition;
                }
            }
        }

        public IEnumerable<Socket> GetSockets()
        {
            foreach (Segment segment in _segments)
            {
                foreach (Socket socket in segment.Sockets)
                {
                    yield return socket;
                }
            }
        }

        /// Expects a segment in world position.
        public bool TryAdd(Segment segment)
        {
            if (Fits(segment))
            {
                _segments.Add(segment);
                return true;
            }

            return false;
        }

        /// Expects a segment in world position.
        public bool Fits(Segment segment)
        {
            if (_segments.Count == 0)
            {
                return true;
            }
            
            HashSet<Position> occupiedPositions = GetOccupiedPositions().ToHashSet();

            foreach (Position position in segment.Positions)
            {
                if (occupiedPositions.Contains(position))
                {
                    // Debug.LogWarning($"Segment has already been occupied by {position}");
                    return false;
                }
            }

            Socket[] existingSockets = GetSockets().ToArray();

            foreach (Socket socket in segment.Sockets)
            {
                foreach (Socket other in existingSockets)
                {
                    if (socket.ConnectsTo(other))
                        return true; // just one socket needs to connect
                }
            }
            
            // Debug.LogWarning($"No sockets found for {segment}");
            return false;
        }
    }
}