using System;

namespace Version_1
{
    public static class SegmentExtensions
    {
        public static Segment Rotate(this Segment segment, Axis axis, Position pivot)
        {
            var positions = new Position[segment.Positions.Count];
            var sockets = new Socket[segment.Sockets.Count];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = segment.Positions[i].RotateAround(pivot, axis);
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                var socket = segment.Sockets[i];

                sockets[i] = new Socket
                {
                    Position = socket.Position.RotateAround(pivot, axis),
                    Direction = socket.Direction.Rotate(axis),
                    Archetype = socket.Archetype
                };
            }

            return new Segment
            {
                Positions = positions,
                Sockets = sockets
            };
        }

        public static Segment Translate(this Segment segment, Position translation)
        {
            var positions = new Position[segment.Positions.Count];
            var sockets = new Socket[segment.Sockets.Count];

            for (var i = 0; i < positions.Length; i++)
            {
                positions[i] = segment.Positions[i] + translation;
            }

            for (int i = 0; i < sockets.Length; i++)
            {
                sockets[i] = new Socket
                {
                    Position = segment.Sockets[i].Position + translation,
                    Direction = segment.Sockets[i].Direction,
                    Archetype = segment.Sockets[i].Archetype,
                };
            }

            return new Segment
            {
                Positions = positions,
                Sockets = sockets,
            };
        }
    }
}