using System;

namespace Version_1
{
    public static class SegmentExtensions
    {
        public static Segment Rotate(this Segment segment, Axis axis, Position pivot)
        {
            Position[] positions = new Position[segment.Positions.Length];
            Socket[] sockets = new Socket[segment.Sockets.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = segment.Positions[i].RotateAround(pivot, axis);
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                Socket socket = segment.Sockets[i];

                sockets[i] = new Socket(
                    socket.Position.RotateAround(pivot, axis), 
                    socket.Direction.Rotate(axis),
                    socket.Archetype);
            }

            return new Segment
            {
                Positions = positions,
                Sockets = sockets
            };
        }

        public static Segment Translate(this Segment segment, Position translation)
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
            };
        }
    }
}