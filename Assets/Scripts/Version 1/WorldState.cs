using System.Collections.Generic;
using System.Linq;

namespace Version_1
{
    public sealed class WorldState
    {
        private readonly HashSet<Segment> _segments = new();
        
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

        // expects a segment in world position
        public void AddSegment(Segment segment)
        {
            if (!Fits(segment))
                return;
            
            _segments.Add(segment);
        }

        // expects a segment in world position
        public bool Fits(Segment segment)
        {
            var occupiedPositions = GetOccupiedPositions().ToHashSet();
            
            foreach (var position in segment.Positions)
            {
                if (occupiedPositions.Contains(position))
                {
                    return false;
                }
            }

            var existingSockets = GetSockets().ToList();
            foreach (var socket in segment.Sockets)
            {
                foreach (var other in existingSockets)
                {
                    if (Math.Connects(socket, other))
                        return true;
                }
            }
            
            return false;
        }
    }
}