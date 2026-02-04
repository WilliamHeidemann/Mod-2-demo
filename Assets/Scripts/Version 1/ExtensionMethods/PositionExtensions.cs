using System;

namespace Version_1
{
    public static class PositionExtensions
    {
        public static Position Rotate(this Position p, Axis axis)
            => axis switch
            {
                Axis.X => new Position(p.X, p.Z, -p.Y),
                Axis.Y => new Position(-p.Z, p.Y, p.X),
                Axis.Z => new Position(p.Y, -p.X, p.Z),
                _ => throw new ArgumentOutOfRangeException(nameof(axis))
            };

        public static Position RotateAround(this Position p, Position pivot, Axis axis)
        {
            // move to origin
            var translated = new Position(
                p.X - pivot.X,
                p.Y - pivot.Y,
                p.Z - pivot.Z);

            // rotate
            var rotated = translated.Rotate(axis);

            // move back
            return new Position(
                rotated.X + pivot.X,
                rotated.Y + pivot.Y,
                rotated.Z + pivot.Z);
        }
    }
}