using System;
using System.Collections.Generic;

namespace Version_1
{
    public static class SegmentExtensions
    {
        public static Segment Rotate(this Segment segment, Axis axis)
        {
            Position[] positions = new Position[segment.Positions.Length];
            Socket[] sockets = new Socket[segment.Sockets.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = segment.Positions[i].RotateAround(segment.Pivot, axis);
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                Socket socket = segment.Sockets[i];

                sockets[i] = new Socket(
                    socket.Position.RotateAround(segment.Pivot, axis), 
                    socket.Direction.Rotate(axis),
                    socket.Archetype);
            }

            return new Segment
            {
                Positions = positions,
                Sockets = sockets,
                Pivot = segment.Pivot
            };
        }

        public static Segment Translate(this Segment segment, Position translation, bool keepPivot = false)
        {
            Position[] positions = new Position[segment.Positions.Length];
            Socket[] sockets = new Socket[segment.Sockets.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = segment.Positions[i] + translation;
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                sockets[i] = new Socket(
                    segment.Sockets[i].Position + translation,
                    segment.Sockets[i].Direction,
                    segment.Sockets[i].Archetype);
            }

            return new Segment
            {
                Positions = positions,
                Sockets = sockets,
                Pivot = keepPivot ? segment.Pivot : segment.Pivot + translation
            };
        }
        
        public static IEnumerable<Segment> GetAllStates(this Segment segment)
        {
            // try all the segment's positions and return all rotations of the segment in that position

            foreach (Position segmentPosition in segment.Positions)
            {
                Segment translatedSegment = segment.Translate(-segmentPosition + segment.Pivot, keepPivot: true);

                foreach (Segment rotatedSegment in translatedSegment.GetStatesInPivot())
                {
                    yield return rotatedSegment;
                }
            }
        }
        
        public static IEnumerable<Segment> GetStatesInPivot(this Segment segment)
        {
            Segment current = segment;

            // There are 6 possible "forward" directions
            for (int face = 0; face < 6; face++)
            {
                // For each face, there are 4 possible "up" directions (90-degree rolls)
                for (int roll = 0; roll < 4; roll++)
                {
                    yield return current;
                    current = current.Rotate(Axis.X);
                }

                // Change the face orientation to move to the next one
                if (face < 3)
                {
                    current = current.Rotate(Axis.Y);
                }
                else if (face == 3)
                {
                    // After 4 faces around the Y axis, move to the "top" face
                    current = current.Rotate(Axis.Z);
                }
                else if (face == 4)
                {
                    // Move from "top" to "bottom" (180 degrees total from the top)
                    current = current.Rotate(Axis.Z)
                        .Rotate(Axis.Z);
                }
            }
        }
    }
}